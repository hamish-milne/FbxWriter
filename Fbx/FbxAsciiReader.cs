using System;
using System.Text;
using System.IO;

namespace Fbx
{
	/// <summary>
	/// Reads FBX nodes from a text stream
	/// </summary>
	public class FbxAsciiReader
	{
		private readonly Stream stream;

		/// <summary>
		/// Creates a new reader
		/// </summary>
		/// <param name="stream"></param>
		public FbxAsciiReader(Stream stream)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));
			this.stream = stream;
		}

		/// <summary>
		/// The maximum array size that will be allocated
		/// </summary>
		/// <remarks>
		/// If you trust the source, you can expand this value as necessary.
		/// Malformed files could cause large amounts of memory to be allocated
		/// and slow or crash the system as a result.
		/// </remarks>
		public int MaxArrayLength { get; set; } = (1 << 24);

		// We read bytes a lot, so we should make a more efficient method here
		// (The normal one makes a new byte array each time)

		readonly byte[] singleChar = new byte[1];
		private char? prevChar;
		private bool endStream;

		// Reads a char, allows peeking and checks for end of stream
		char ReadChar()
		{
			if (prevChar != null)
			{
				var c = prevChar.Value;
				prevChar = null;
				return c;
			}
			if (stream.Read(singleChar, 0, 1) < 1)
			{
				endStream = true;
				return '\0';
			}
			return (char)singleChar[0];
		}

		// Checks if a character is valid in a real number
		static bool IsDigit(char c, bool first)
		{
			if (char.IsDigit(c))
				return true;
			switch (c)
			{
				case '-':
				case '+':
					return true;
				case '.':
				case 'e':
				case 'E':
				case 'X':
				case 'x':
					return !first;
			}
			return false;
		}

		static bool IsLineEnd(char c)
		{
			return c == '\r' || c == '\n';
		}

		// Token to mark the end of the stream
		class EndOfStream
		{
		}

		// Wrapper around a string to mark it as an identifier
		// (as opposed to a string literal)
		class Identifier
		{
			public readonly string String;

			public override bool Equals(object obj)
			{
				var id = obj as Identifier;
				if (id != null)
					return String == id.String;
				return false;
			}

			public override int GetHashCode()
			{
				return String?.GetHashCode() ?? 0;
			}

			public Identifier(string str)
			{
				String = str;
			}
		}

		private object prevTokenSingle;

		// Reads a single token, allows peeking
		// Can return 'null' for a comment or whitespace
		object ReadTokenSingle()
		{
			if (prevTokenSingle != null)
			{
				var ret = prevTokenSingle;
				prevTokenSingle = null;
				return ret;
			}
			var c = ReadChar();
			if(endStream)
				return new EndOfStream();
			switch (c)
			{
				case ';': // Comments
					while (!IsLineEnd(ReadChar()) && !endStream) { } // Skip a line
					return null;
				case '{': // Operators
				case '}':
				case '*':
				case ':':
				case ',':
					return c;
				case '"': // String literal
					var sb1 = new StringBuilder();
					while((c = ReadChar()) != '"')
					{
						if (endStream)
							throw new FbxException(stream.Position,
								"Unexpected end of stream; expecting end quote");
						sb1.Append(c);
					}
					return sb1.ToString();
				default:
					if (char.IsWhiteSpace(c))
					{
						// Merge whitespace
						while (char.IsWhiteSpace(c = ReadChar()) && !endStream) { }
						if (!endStream)
							prevChar = c;
						return null;
					}
					if (IsDigit(c, true)) // Number
					{
						var sb2 = new StringBuilder();
						do
						{
							sb2.Append(c);
							c = ReadChar();
						} while (IsDigit(c, false) && !endStream);
						if(!endStream)
							prevChar = c;
						var str = sb2.ToString();
						if (str.Contains("."))
						{
							// TODO: Separate float and double
							double d;
							if (!double.TryParse(str, out d))
								throw new FbxException(stream.Position,
									"Invalid number");
							return d;
						}
						long l;
						if (!long.TryParse(str, out l))
							throw new FbxException(stream.Position,
								"Invalid integer");
						// Check size and return the smallest possible
						if (l >= byte.MinValue && l <= byte.MaxValue)
							return (byte) l;
						if (l >= int.MinValue && l <= int.MaxValue)
							return (int) l;
						return l;
					}
					if (char.IsLetter(c) || c == '_') // Identifier
					{
						var sb3 = new StringBuilder();
						do
						{
							sb3.Append(c);
							c = ReadChar();
						} while ((char.IsLetterOrDigit(c) || c == '_') && !endStream);
						if(!endStream)
							prevChar = c;
						return new Identifier(sb3.ToString());
					}
					break;
			}
			throw new FbxException(stream.Position,
				"Unknown character " + c);
		}

		private object prevToken;

		// Use a loop rather than recursion to prevent stack overflow
		// Here we can also merge string+colon into an identifier,
		// returning single-character bare strings (for C-type properties)
		object ReadToken()
		{
			object ret;
			if (prevToken != null)
			{
				ret = prevToken;
				prevToken = null;
				return ret;
			}
			do
			{
				ret = ReadTokenSingle();
			} while (ret == null);
			var id = ret as Identifier;
			if (id != null)
			{
				object colon;
				do
				{
					colon = ReadTokenSingle();
				} while (colon == null);
				if (!':'.Equals(colon))
				{
					if (id.String.Length > 1)
						throw new FbxException(stream.Position,
							"Unexpected " + colon + ", expected ':' or a single-char literal");
					ret = id.String[0];
					prevTokenSingle = colon;
				}
			}
			return ret;
		}

		void ExpectToken(object token)
		{
			var t = ReadToken();
            if (!token.Equals(t))
				throw new FbxException(stream.Position,
					"Unexpected " + t + ", expected " + token);
		}

		private enum ArrayType
		{
			Byte = 0,
			Int = 1,
			Long = 2,
			Float = 3,
		};

		Array ReadArray()
		{
			// Read array length and header
			var len = ReadToken();
			long l;
			if (len is long)
				l = (long) len;
			else if (len is int)
				l = (int) len;
			else if (len is byte)
				l = (byte) len;
			else
				throw new FbxException(stream.Position,
					"Unexpected " + len + ", expected an integer");
			if(l < 0)
				throw new FbxException(stream.Position,
					"Invalid array length " + l);
			if(l > MaxArrayLength)
				throw new FbxException(stream.Position,
					"Array length " + l + " higher than permitted maximum " + MaxArrayLength);
			ExpectToken('{');
			ExpectToken(new Identifier("a"));
			var array = new double[l];

			// Read array elements
			bool expectComma = false;
			object token;
			var arrayType = ArrayType.Byte;
			long pos = 0;
			while (!'}'.Equals(token = ReadToken()))
			{
				if (expectComma)
				{
					if (!','.Equals(token))
						throw new FbxException(stream.Position,
							"Unexpected " + token + ", expected a ','");
					expectComma = false;
					continue;
				}
				if(pos >= array.Length)
					throw new FbxException(stream.Position,
						"Too many elements in array");

				// Add element to the array, checking for the maximum
				// size of any one element.
				// (I'm not sure if this is the 'correct' way to do it, but it's the only
				// logical one given the nature of the ASCII format)
				bool isDouble = token is double;
				double d;
				if (!isDouble)
				{
					if (token is byte)
					{
						d = (byte) token;
					} else if (token is int)
					{
						d = (int) token;
						if(arrayType < ArrayType.Int)
							arrayType = ArrayType.Int;
					} else if (token is long)
					{
						d = (long) token;
						if (arrayType < ArrayType.Long)
							arrayType = ArrayType.Long;
					} else
						throw new FbxException(stream.Position,
								"Unexpected " + token + ", expected a number");
				} else
				{
					d = (double) token;
					if (arrayType < ArrayType.Float)
						arrayType = ArrayType.Float;
				}
				array[pos++] = d;
				expectComma = true;
			}
			
			// Convert the array to the smallest type we can see
			Array ret;
			switch (arrayType)
			{
				case ArrayType.Byte:
					var bArray = new byte[array.Length];
					for (int i = 0; i < bArray.Length; i++)
						bArray[i] = (byte)array[i];
					ret = bArray;
					break;
				case ArrayType.Int:
					var intArray = new int[array.Length];
					for (int i = 0; i < intArray.Length; i++)
						intArray[i] = (int)array[i];
					ret = intArray;
					break;
				case ArrayType.Long:
					var lArray = new long[array.Length];
					for (int i = 0; i < lArray.Length; i++)
						lArray[i] = (long)array[i];
					ret = lArray;
					break;
				default:
					ret = array;
					break;
			}
			return ret;
		}

		/// <summary>
		/// Reads the next node from the stream
		/// </summary>
		/// <returns>The read node, or <c>null</c></returns>
		public FbxNode ReadNode()
		{
			var first = ReadToken();
			var id = first as Identifier;
			if (id == null)
			{
				if (first is EndOfStream)
					return null;
				throw new FbxException(stream.Position, "Expected an identifier");
			}
			var node = new FbxNode {Name = id.String};

			// Read properties
			object token;
			bool expectComma = false;
			while (!'{'.Equals(token = ReadToken()) && !(token is Identifier) && !'}'.Equals(token))
			{
				if (expectComma)
				{
					if (!','.Equals(token)) 
						throw new FbxException(stream.Position,
							"Unexpected " + token + ", expected a ','");
					expectComma = false;
					continue;
				}
				if (token is char)
				{
					var c = (char) token;
					switch (c)
					{
						case '*':
							token = ReadArray();
							break;
						case '}':
						case ':':
						case ',':
							throw new FbxException(stream.Position,
								"Unexpected " + c + " in property list");
					}
				}
				node.Properties.Add(token);
				expectComma = true; // The final comma before the open brace isn't required
			}
			// TODO: Merge property list into an array as necessary
			// Now we're either at an open brace, close brace or a new node
			if (token is Identifier || '}'.Equals(token))
			{
				prevToken = token;
				return node;
			}
			// The while loop can't end unless we're at an open brace, so we can continue right on
			object endBrace;
			while(!'}'.Equals(endBrace = ReadToken()))
			{
				prevToken = endBrace; // If it's not an end brace, the next node will need it
				node.Nodes.Add(ReadNode());
			}
			if(node.Nodes.Count < 1) // If there's an open brace, we want that to be preserved
				node.Nodes.Add(null);
			return node;
		}

		/// <summary>
		/// Reads a full document from the stream
		/// </summary>
		/// <returns>The complete document object</returns>
		public FbxDocument Read()
		{
			var ret = new FbxDocument();
			// TODO: Read version string
			FbxNode node;
			while((node = ReadNode()) != null)
				ret.Nodes.Add(node);
			return ret;
		}

	}
}

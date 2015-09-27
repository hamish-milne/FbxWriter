using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Fbx
{
	/// <summary>
	/// Writes an FBX document to a binary stream
	/// </summary>
	public class FbxBinaryWriter : FbxBinary
	{
		private readonly Stream output;
		private readonly MemoryStream memory;
		private readonly BinaryWriter stream;

		/// <summary>
		/// The minimum size of an array in bytes before it is compressed
		/// </summary>
		public int CompressionThreshold { get; set; } = 1024;

		/// <summary>
		/// Creates a new writer
		/// </summary>
		/// <param name="stream"></param>
		public FbxBinaryWriter(Stream stream)
		{
			if(stream == null)
				throw new ArgumentNullException(nameof(stream));
			output = stream;
			// Wrap in a memory stream to guarantee seeking
			memory = new MemoryStream();
            this.stream = new BinaryWriter(memory, Encoding.ASCII);
		}

		private delegate void PropertyWriter(BinaryWriter sw, object obj);

		struct WriterInfo
		{
			public readonly char id;
			public readonly PropertyWriter writer;

			public WriterInfo(char id, PropertyWriter writer)
			{
				this.id = id;
				this.writer = writer;
			}
		}

		private static readonly Dictionary<Type, WriterInfo> writePropertyActions
			= new Dictionary<Type, WriterInfo>
			{
				{ typeof(int),    new WriterInfo('I', (sw, obj) => sw.Write((int)obj)) },
				{ typeof(short),  new WriterInfo('Y', (sw, obj) => sw.Write((short)obj)) },
				{ typeof(long),   new WriterInfo('L', (sw, obj) => sw.Write((long)obj)) },
				{ typeof(float),  new WriterInfo('F', (sw, obj) => sw.Write((float)obj)) },
				{ typeof(double), new WriterInfo('D', (sw, obj) => sw.Write((double)obj)) },
				{ typeof(bool),   new WriterInfo('C', (sw, obj) => sw.Write((byte)(char)obj)) },
				{ typeof(byte[]), new WriterInfo('R', WriteRaw) },
				{ typeof(string), new WriterInfo('S', WriteString) },
				// null elements indicate arrays - they are checked again with their element type
				{ typeof(int[]),    new WriterInfo('i', null) },
				{ typeof(long[]),   new WriterInfo('l', null) },
				{ typeof(float[]),  new WriterInfo('f', null) },
				{ typeof(double[]), new WriterInfo('d', null) },
				{ typeof(bool[]),   new WriterInfo('b', null) },
			};

		static void WriteRaw(BinaryWriter stream, object obj)
		{
			var bytes = (byte[]) obj;
			stream.Write(bytes.Length);
			stream.Write(bytes);
		}

		static void WriteString(BinaryWriter stream, object obj)
		{
			var str = obj.ToString();
			// Replace "::" with \0\1 and reverse the tokens
			if (str.Contains(asciiSeparator))
			{
				var tokens = str.Split(new []{ asciiSeparator }, StringSplitOptions.None);
				var sb = new StringBuilder();
				bool first = true;
				for (int i = tokens.Length - 1; i >= 0; i--)
				{
					if (!first)
						sb.Append(binarySeparator);
					sb.Append(tokens[i]);
					first = false;
				}
				str = sb.ToString();
			}
            var bytes = Encoding.ASCII.GetBytes(str);
			stream.Write(bytes.Length);
			stream.Write(bytes);
		}

		void WriteArray(Array array, Type elementType, PropertyWriter writer)
		{
			stream.Write(array.Length);
			
			var size = array.Length*Marshal.SizeOf(elementType);
            bool compress = size >= CompressionThreshold;
			stream.Write(compress ? 1 : 0);

			var sw = stream;
			DeflateWithChecksum codec = null;

			var compressLengthPos = stream.BaseStream.Position;
			stream.Write(0); // Placeholder compressed length
			var dataStart = stream.BaseStream.Position;
			if (compress)
			{
				stream.Write(new byte[] { 0x58, 0x85 }, 0, 2); // Header bytes for DeflateStream settings
				codec = new DeflateWithChecksum(stream.BaseStream, CompressionMode.Compress, true);
				sw = new BinaryWriter(codec);
			}
			foreach (var obj in array)
				writer(sw, obj);
			if (compress)
			{
				codec.Close(); // This is important - otherwise bytes can be incorrect
				var checksum = codec.Checksum;
				byte[] bytes =
				{
					(byte)((checksum >> 24) & 0xFF),
					(byte)((checksum >> 16) & 0xFF),
					(byte)((checksum >> 8) & 0xFF),
					(byte)(checksum & 0xFF),
				};
				stream.Write(bytes);
			}
			
			// Now we can write the compressed data length, since we know the size
			if (compress)
			{
				var dataEnd = stream.BaseStream.Position;
				stream.BaseStream.Position = compressLengthPos;
				stream.Write((int)(dataEnd - dataStart));
				stream.BaseStream.Position = dataEnd;
			}
		}

		void WriteProperty(object obj)
		{
			if (obj == null)
				return;
			WriterInfo writerInfo;
			if(!writePropertyActions.TryGetValue(obj.GetType(), out writerInfo))
				throw new ArgumentException("Invalid property type " + obj.GetType());
			stream.Write((byte)writerInfo.id);
			// ReSharper disable once AssignNullToNotNullAttribute
			if (writerInfo.writer == null) // Array type
			{
				var elementType = obj.GetType().GetElementType();
                WriteArray((Array) obj, elementType, writePropertyActions[elementType].writer);
			} else
				writerInfo.writer(stream, obj);
		}

		// Data for a null node
		static readonly byte[] nullData = new byte[13];

		// Writes a single document to the buffer
		void WriteNode(FbxNode node)
		{
			if (node == null)
			{
				stream.BaseStream.Write(nullData, 0, nullData.Length);
			} else
			{
				var name = string.IsNullOrEmpty(node.Name) ? null : Encoding.ASCII.GetBytes(node.Name);
				if(name != null && name.Length > byte.MaxValue)
					throw new FbxException(stream.BaseStream.Position,
						"Node name '" + node.Name + "' is too long");

				// Header
				var endOffsetPos = stream.BaseStream.Position;
				stream.Write(0); // End offset placeholder
				stream.Write(node.Properties.Count);
				var propertyLengthPos = stream.BaseStream.Position;
				stream.Write(0); // Property length placeholder
				stream.Write((byte)(name?.Length ?? 0));
				if(name != null)
					stream.Write(name);

				// Write properties and length
				var propertyBegin = stream.BaseStream.Position;
				foreach (var p in node.Properties)
				{
					WriteProperty(p);
				}
				var propertyEnd = stream.BaseStream.Position;
				stream.BaseStream.Position = propertyLengthPos;
				stream.Write((int)(propertyEnd - propertyBegin));
				stream.BaseStream.Position = propertyEnd;

				// Write child nodes
				if (node.Nodes.Count > 0)
				{
					foreach (var n in node.Nodes)
					{
						if(n == null)
							continue;
						WriteNode(n);
					}
					WriteNode(null);
				}

				// Write end offset
				var dataEnd = stream.BaseStream.Position;
				stream.BaseStream.Position = endOffsetPos;
				stream.Write((int)dataEnd);
				stream.BaseStream.Position = dataEnd;
			}
		}

		/// <summary>
		/// Writes an FBX file to the output
		/// </summary>
		/// <param name="document"></param>
		public void Write(FbxDocument document)
		{
			stream.BaseStream.Position = 0;
			WriteHeader(stream.BaseStream);
			stream.Write((int)document.Version);
			// TODO: Do we write a top level node or not? Maybe check the version?
			foreach (var node in document.Nodes)
				WriteNode(node);
			WriteNode(null);
			stream.Write(GenerateFooterCode(document, stream.BaseStream.Position));
			WriteFooter(stream, (int)document.Version);
			output.Write(memory.GetBuffer(), 0, (int)memory.Position);
		}
	}
}

using System;
using System.Text;
using System.IO;

namespace Fbx
{
	/// <summary>
	/// Writes an FBX document in a text format
	/// </summary>
	public class FbxAsciiWriter
	{
		private readonly Stream stream;

		/// <summary>
		/// Creates a new reader
		/// </summary>
		/// <param name="stream"></param>
		public FbxAsciiWriter(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			this.stream = stream;
		}

		// Adds the given node text to the string
		void BuildString(FbxNode node, StringBuilder sb, bool writeArrayLength, int indentLevel = 0)
		{
			for (int i = 0; i < indentLevel; i++)
				sb.Append('\t');
			sb.Append(node.Name).Append(':');
			var first = true;
			foreach (var p in node.Properties)
			{
				if(p == null)
					continue;
				if (!first)
					sb.Append(',');
				sb.Append(' ');
				if (p is string)
				{
					sb.Append('"').Append(p).Append('"');
				} else if (p is Array)
				{
					var array = (Array) p;
					// ReSharper disable once PossibleNullReferenceException
					// We know it's an array, so we don't need to check for null
					if (array.Rank != 1 || !p.GetType().GetElementType().IsPrimitive)
						throw new FbxException(-1, "Invalid array type " + p.GetType());
					if (writeArrayLength)
					{
						sb.Append('*').Append(array.Length).Append(" {\n");
						for (int i = -1; i < indentLevel; i++)
							sb.Append('\t');
						sb.Append("a: ");
					}
					bool pFirst = true;
					foreach (var v in (Array) p)
					{
						if (!pFirst)
							sb.Append(',');
						sb.Append(v);
						pFirst = false;
					}
					if (writeArrayLength)
					{
						sb.Append('\n');
						for (int i = 0; i < indentLevel; i++)
							sb.Append('\t');
						sb.Append('}');
					}
				} else if (p is char)
					sb.Append((char) p);
				else if(p.GetType().IsPrimitive && p is IFormattable)
					sb.Append(p);
				else
					throw new FbxException(-1, "Invalid property type " + p.GetType());
				first = false;
			}
			if (node.Nodes.Count > 0)
			{
				sb.Append(" {\n");
				foreach (var n in node.Nodes)
				{
					if (n == null)
						continue;
					BuildString(n, sb, writeArrayLength, indentLevel + 1);
				}
				for (int i = 0; i < indentLevel; i++)
					sb.Append('\t');
				sb.Append('}');
			}
			sb.Append('\n');
		}

		/// <summary>
		/// Writes an FBX document to the stream
		/// </summary>
		/// <param name="document"></param>
		/// <remarks>
		/// ASCII FBX files have no header or footer, so you can call this multiple times
		/// </remarks>
		public void Write(FbxDocument document)
		{
			if(document == null)
				throw new ArgumentNullException(nameof(document));
			var sb = new StringBuilder();

			// Write version header (a comment, but required for many importers)
			var vMajor = (int)document.Version/1000;
			var vMinor = ((int) document.Version%1000)/100;
			var vRev = ((int) document.Version%100)/10;
			sb.Append($"; FBX {vMajor}.{vMinor}.{vRev} project file\n\n");

			foreach (var n in document.Nodes)
			{
				if (n == null)
					continue;
				BuildString(n, sb, document.Version >= FbxVersion.v7_1);
				sb.Append('\n');
			}
			var b = Encoding.ASCII.GetBytes(sb.ToString());
			stream.Write(b, 0, b.Length);
		}
	}
}

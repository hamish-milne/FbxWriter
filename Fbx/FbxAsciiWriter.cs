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
		void BuildString(FbxNode node, StringBuilder sb, int indentLevel = 0)
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
					// ReSharper disable once PossibleNullReferenceException
					// We know it's an array, so we don't need to check for null
					if (!p.GetType().GetElementType().IsPrimitive)
						throw new ArgumentException("Invalid array element");
					bool pFirst = true;
					foreach (var v in (Array) p)
					{
						if (pFirst)
							sb.Append(v);
						else
							sb.Append(',').Append(v);
						pFirst = false;
					}
				}
				else if (p.GetType().IsPrimitive)
					sb.Append(p);
				else
					throw new ArgumentException("Invalid property item");
				first = false;
			}
			if (node.Nodes.Count > 0)
			{
				sb.Append(" {\n");
				foreach (var n in node.Nodes)
				{
					if (n == null)
						continue;
					BuildString(n, sb, indentLevel + 1);
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
		/// <param name="topLevel"></param>
		/// <remarks>
		/// ASCII FBX files have no header or footer, so you can call this multiple times
		/// </remarks>
		public void Write(FbxNode topLevel)
		{
			var sb = new StringBuilder();
			foreach (var n in topLevel.Nodes)
			{
				if (n == null)
					continue;
				BuildString(n, sb);
				sb.Append('\n');
			}
			var b = Encoding.ASCII.GetBytes(sb.ToString());
			stream.Write(b, 0, b.Length);
		}
	}
}

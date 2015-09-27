using System.IO;

namespace Fbx
{
	/// <summary>
	/// Quick utility methods
	/// </summary>
	public static class FbxUtil
	{
		/// <summary>
		/// Reads an FBX file
		/// </summary>
		/// <param name="path"></param>
		/// <returns>The top level document node</returns>
		public static FbxNode ReadBinary(string path)
		{
			using (var stream = new FileStream(path, FileMode.Open))
			{
				var reader = new FbxBinaryReader(stream);
				int version;
				return reader.Read(out version);
			}
		}

		/// <summary>
		/// Writes an FBX document
		/// </summary>
		/// <param name="document">The top level document node</param>
		/// <param name="path"></param>
		public static void WriteBinary(FbxNode document, string path)
		{
			using (var stream = new FileStream(path, FileMode.Create))
			{
				var writer = new FbxBinaryWriter(stream);
				writer.Write(document);
			}
		}

		/// <summary>
		/// Writes an FBX document
		/// </summary>
		/// <param name="document">The top level document node</param>
		/// <param name="path"></param>
		public static void WriteAscii(FbxNode document, string path)
		{
			using (var stream = new FileStream(path, FileMode.Create))
			{
				var writer = new FbxAsciiWriter(stream);
				writer.Write(document);
			}
		}
	}
}

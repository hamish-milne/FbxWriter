using System.IO;
using System.IO.Compression;

namespace Fbx
{
	/// <summary>
	/// A wrapper for DeflateStream that calculates the Adler32 checksum of the payload
	/// </summary>
	public class DeflateWithChecksum : DeflateStream
	{
		private const int modAdler = 65521;
		private uint checksumA;
		private uint checksumB;

		/// <summary>
		/// Gets the Adler32 checksum at the current point in the stream
		/// </summary>
		public int Checksum
		{
			get
			{
				checksumA %= modAdler;
				checksumB %= modAdler;
				return (int)((checksumB << 16) | checksumA);
			}
		}

		/// <inheritdoc />
		public DeflateWithChecksum(Stream stream, CompressionMode mode) : base(stream, mode)
		{
			ResetChecksum();
		}

		/// <inheritdoc />
		public DeflateWithChecksum(Stream stream, CompressionMode mode, bool leaveOpen) : base(stream, mode, leaveOpen)
		{
			ResetChecksum();
		}

		// Efficiently extends the checksum with the given buffer
		void CalcChecksum(byte[] array, int offset, int count)
		{
			checksumA %= modAdler;
			checksumB %= modAdler;
			for (int i = offset, c = 0; i < (offset + count); i++, c++)
			{
				checksumA += array[i];
				checksumB += checksumA;
				if (c > 4000) // This is about how many iterations it takes for B to reach IntMax
				{
					checksumA %= modAdler;
					checksumB %= modAdler;
					c = 0;
				}
			}
		}

		/// <inheritdoc />
		public override void Write(byte[] array, int offset, int count)
		{
			base.Write(array, offset, count);
			CalcChecksum(array, offset, count);
		}

		/// <inheritdoc />
		public override int Read(byte[] array, int offset, int count)
		{
			var ret = base.Read(array, offset, count);
			CalcChecksum(array, offset, count);
			return ret;
		}

		/// <summary>
		/// Initializes the checksum values
		/// </summary>
		public void ResetChecksum()
		{
			checksumA = 1;
			checksumB = 0;
		}
	}
}

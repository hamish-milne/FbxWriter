using System.IO;
using System.IO.Compression;

namespace Fbx
{
	public class DeflateWithChecksum : DeflateStream
	{
		private const int modAdler = 65521;
		private uint checksumA;
		private uint checksumB;

		public int Checksum
		{
			get
			{
				checksumA %= modAdler;
				checksumB %= modAdler;
				return (int)((checksumB << 16) | checksumA);
			}
		}

		public DeflateWithChecksum(Stream stream, CompressionMode mode) : base(stream, mode)
		{
			ResetChecksum();
		}

		public DeflateWithChecksum(Stream stream, CompressionMode mode, bool leaveOpen) : base(stream, mode, leaveOpen)
		{
			ResetChecksum();
		}

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

		public override void Write(byte[] array, int offset, int count)
		{
			base.Write(array, offset, count);
			CalcChecksum(array, offset, count);
		}

		public override int Read(byte[] array, int offset, int count)
		{
			var ret = base.Read(array, offset, count);
			CalcChecksum(array, offset, count);
			return ret;
		}

		public void ResetChecksum()
		{
			checksumA = 1;
			checksumB = 0;
		}
	}
}

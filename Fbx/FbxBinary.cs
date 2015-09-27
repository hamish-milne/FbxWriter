using System;
using System.IO;
using System.Text;

namespace Fbx
{
	public abstract class FbxBinary
	{
		private static readonly byte[] headerString
			= Encoding.ASCII.GetBytes("Kaydara FBX Binary  \0\x1a\0");

		// This data was entirely calculated by me. Turns out it works, fancy that!
		private static readonly byte[] sourceId =  { 0x58, 0xAB, 0xA9, 0xF0, 0x6C, 0xA2, 0xD8, 0x3F, 0x4D, 0x47, 0x49, 0xA3, 0xB4, 0xB2, 0xE7, 0x3D };
		private static readonly byte[] key =       { 0xE2, 0x4F, 0x7B, 0x5F, 0xCD, 0xE4, 0xC8, 0x6D, 0xDB, 0xD8, 0xFB, 0xD7, 0x40, 0x58, 0xC6, 0x78 };
		// This wasn't - it just appears at the end of every compliant file
		private static readonly byte[] extension = { 0xF8, 0x5A, 0x8C, 0x6A, 0xDE, 0xF5, 0xD9, 0x7E, 0xEC, 0xE9, 0x0C, 0xE3, 0x75, 0x8F, 0x29, 0x0B };

		private const int footerZeroes1 = 20;
		private const int footerZeroes2 = 120;

		protected const int footerCodeSize = 16;

		protected static bool CheckEqual(byte[] data, byte[] original)
		{
			for (int i = 0; i < original.Length; i++)
				if (data[i] != original[i])
					return false;
			return true;
		}

		protected static void WriteHeader(Stream stream)
		{
			stream.Write(headerString, 0, headerString.Length);
		}

		protected static bool ReadHeader(Stream stream)
		{
			var buf = new byte[headerString.Length];
			stream.Read(buf, 0, buf.Length);
			return CheckEqual(buf, headerString);
		}

		// Turns out this is the algorithm they use to generate the footer. Who knew!
		static void Encrypt(byte[] a, byte[] b)
		{
			byte c = 64;
			for (int i = 0; i < footerCodeSize; i++)
			{
				a[i] = (byte)(a[i] ^ (byte)(c ^ b[i]));
				c = a[i];
			}
		}

		static int GetTimestampVar(FbxNode timestamp, long dataPos, string element)
		{
			var elementNode = timestamp[element];
			if (elementNode != null && elementNode.Properties.Count > 0)
			{
				var prop = elementNode.Properties[0];
				if (prop is int)
					return (int)prop;
			}
			throw new FbxException(dataPos, "Timestamp has no " + element);
		}

		protected static byte[] GenerateFooterCode(FbxNode document, long dataPos)
		{
			var timestamp = document.GetRelative("FBXHeaderExtension/CreationTimeStamp");
			if (timestamp == null)
				throw new FbxException(dataPos, "No creation timestamp");
			return GenerateFooterCode(
					GetTimestampVar(timestamp, dataPos, "Year"),
					GetTimestampVar(timestamp, dataPos, "Month"),
					GetTimestampVar(timestamp, dataPos, "Day"),
					GetTimestampVar(timestamp, dataPos, "Hour"),
					GetTimestampVar(timestamp, dataPos, "Minute"),
					GetTimestampVar(timestamp, dataPos, "Second"),
					GetTimestampVar(timestamp, dataPos, "Millisecond")
				);
		}

		protected static byte[] GenerateFooterCode(
			int year, int month, int day,
			int hour, int minute, int second, int millisecond)
		{
			if(year < 0 || year > 9999)
				throw new ArgumentOutOfRangeException(nameof(year));
			if(month < 0 || month > 12)
				throw new ArgumentOutOfRangeException(nameof(month));
			if(day < 0 || day > 31)
				throw new ArgumentOutOfRangeException(nameof(day));
			if(hour < 0 || hour >= 24)
				throw new ArgumentOutOfRangeException(nameof(hour));
			if(minute < 0 || minute >= 60)
				throw new ArgumentOutOfRangeException(nameof(minute));
			if(second < 0 || second >= 60)
				throw new ArgumentOutOfRangeException(nameof(second));
			if(millisecond < 0 || millisecond >= 1000)
				throw new ArgumentOutOfRangeException(nameof(millisecond));

			var str = (byte[]) sourceId.Clone();
			var mangledTime = $"{second:00}{month:00}{hour:00}{day:00}{(millisecond/10):00}{year:0000}{minute:00}";
			var mangledBytes = Encoding.ASCII.GetBytes(mangledTime);
			Encrypt(str, mangledBytes);
			Encrypt(str, key);
			Encrypt(str, mangledBytes);
			return str;
		}

		protected void WriteFooter(BinaryWriter stream, int version)
		{
			var zeroes = new byte[Math.Max(footerZeroes1, footerZeroes2)];
			stream.Write(zeroes, 0, footerZeroes1);
			stream.Write(version);
			stream.Write(zeroes, 0, footerZeroes2);
			stream.Write(extension, 0, extension.Length);
		}

		static bool AllZero(byte[] array)
		{
			foreach(var b in array)
				if (b != 0)
					return false;
			return true;
		}

		protected bool CheckFooter(BinaryReader stream, int version)
		{
			var buffer = new byte[Math.Max(footerZeroes1, footerZeroes2)];
			stream.Read(buffer, 0, footerZeroes1);
			bool correct = AllZero(buffer);
			var readVersion = stream.ReadInt32();
			correct &= (readVersion == version);
			stream.Read(buffer, 0, footerZeroes2);
			correct &= AllZero(buffer);
			stream.Read(buffer, 0, extension.Length);
			correct &= CheckEqual(buffer, extension);
			return correct;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Fbx
{
    class Program
    {
	    static void Encrypt(byte[] a, byte[] b)
	    {
		    byte c = 64;
		    for (int i = 0; i < 16; i++)
		    {
			    a[i] = (byte)(a[i] ^ (byte)(c ^ b[i]));
			    c = a[i];
		    }
	    }

	    static void PrintBytes(byte[] array)
	    {
		    foreach(var b in array)
				Console.Write(b + " ");
			Console.Write("\n");
	    }

	    private static readonly byte[] sourceId = { 0x58, 0xAB, 0xA9, 0xF0, 0x6C, 0xA2, 0xD8, 0x3F, 0x4D, 0x47, 0x49, 0xA3, 0xB4, 0xB2, 0xE7, 0x3D };
	    private static readonly byte[] key =      { 0xE2, 0x4F, 0x7B, 0x5F, 0xCD, 0xE4, 0xC8, 0x6D, 0xDB, 0xD8, 0xFB, 0xD7, 0x40, 0x58, 0xC6, 0x78 };

        static void Main(string[] args)
        {
			Console.WriteLine("Start");
			var stream = new FileStream(args[0], FileMode.Open);
	        int version;
	        var node = (new FbxBinaryReader(stream)).Read(out version);
			stream.Close();

			stream = new FileStream(args[1], FileMode.Create);
			(new FbxBinaryWriter(stream)).Write(node);
			stream.Close();

	        /*var timestamp = node["FBXHeaderExtension"]["CreationTimeStamp"];

	        var year = (int)timestamp["Year"].Properties[0];
			var month = (int)timestamp["Month"].Properties[0];
			var day = (int)timestamp["Day"].Properties[0];
			var hour = (int)timestamp["Hour"].Properties[0];
			var minute = (int)timestamp["Minute"].Properties[0];
			var second = (int)timestamp["Second"].Properties[0];
			var millisecond = (int)timestamp["Millisecond"].Properties[0];

	        var mangledtime = $"{second:00}{month:00}{hour:00}{day:00}{(millisecond/10):00}{year:0000}{minute:00}";

			Console.WriteLine(mangledtime);

	        var mangledBytes = Encoding.ASCII.GetBytes(mangledtime);

			if(mangledBytes.Length != 16)
				throw new Exception();

	        var str = (byte[])sourceId.Clone();
			Encrypt(str, mangledBytes);
			Encrypt(str, key);
			Encrypt(str, mangledBytes);

			PrintBytes(footer);
			PrintBytes(str);*/


			//Console.ReadLine();
        }
    }
}

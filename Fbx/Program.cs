using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Fbx
{
    class Program
    {
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
        }
    }
}

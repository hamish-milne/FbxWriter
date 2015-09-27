using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Fbx;

namespace FbxTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var document = FbxIO.ReadBinary(args[0]);
			FbxIO.WriteAscii(document, Path.GetDirectoryName(args[0]) + "/test_ascii.fbx");
		}
	}
}

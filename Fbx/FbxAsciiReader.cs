using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Fbx
{
	public class FbxAsciiReader
	{
		private Stream stream;

		public FbxAsciiReader(Stream stream)
		{
			this.stream = stream;
		}

		public FbxNode ReadNext()
		{
			return null;
		}
	}
}

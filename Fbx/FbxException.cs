using System;
using System.Collections.Generic;
using System.Text;

namespace Fbx
{
	public class FbxException : Exception
	{
		public FbxException(long position, string message) :
			base(message + ", at offset " + position)
		{
		}
	}
}

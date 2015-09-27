using System;

namespace Fbx
{
	/// <summary>
	/// An error with the FBX data input
	/// </summary>
	public class FbxException : Exception
	{
		public FbxException(long position, string message) :
			base(message + ", at offset " + position)
		{
		}
	}
}

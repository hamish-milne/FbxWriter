using System;

namespace Fbx
{
	/// <summary>
	/// An error with the FBX data input
	/// </summary>
	public class FbxException : Exception
	{
		/// <summary>
		/// Creates a new instance
		/// </summary>
		/// <param name="position"></param>
		/// <param name="message"></param>
		public FbxException(long position, string message) :
			base(message + ", at offset " + position)
		{
		}
	}
}

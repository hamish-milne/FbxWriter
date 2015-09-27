using System.Text;

namespace Fbx
{
	public abstract class FbxBinary
	{
		protected internal static readonly byte[] headerString
			= Encoding.ASCII.GetBytes("Kaydara FBX Binary  \0");
		protected internal const short magic = 26;
	}
}

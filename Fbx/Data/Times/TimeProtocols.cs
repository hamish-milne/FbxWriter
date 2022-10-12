namespace Fbx.Data.Times
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	/// 
	/// The various kinds of time protocols that exist. The default value used seems to be 'DefaultProtocol'.
	/// </summary>
	public enum TimeProtocols
	{
		SMPTE,
		FrameCount,
		DefaultProtocol, // This defaults to FrameCount by the way.
	}
}

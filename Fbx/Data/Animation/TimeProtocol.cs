namespace Fbx.Data.Animation
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	/// 
	/// The various kinds of time protocols that exist. The default value used seems to be 'DefaultProtocol'.
	/// </summary>
	public enum TimeProtocol
	{
		SMPTE,
		FrameCount,
		DefaultProtocol, // This defaults to FrameCount by the way.
	}
}
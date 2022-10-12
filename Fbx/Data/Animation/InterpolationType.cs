using System;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	/// 
	/// The interpolation type that keys can have. This seems to get combined with TangentMode into a single integer.
	/// </summary>
	[Flags]
	public enum InterpolationType
	{
		Constant = 0x00000002,	// Constant value until next key.
		Linear = 0x00000004,	// Linear progression to next key.
		Cubic = 0x00000008		// Cubic progression to next key.
	}
}

using System;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	///
	/// Flags for whether a keyframe's tangents use velocity or not.
	/// </summary>
	[Flags]
	public enum VelocityModes
	{
		None = 0x00000000,		// No velocity (default).
		Right = 0x10000000,		// Right tangent has velocity.
		NextLeft = 0x20000000,	// Left tangent has velocity.
		All = Right|NextLeft	// Both left and right tangents have velocity.
	};
}

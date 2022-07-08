using System;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	///
	/// The types of tangents that a constant keyframe can have.
	/// </summary>
	[Flags]
	public enum ConstantModes
	{
		ConstantStandard = 0x00000000,	// Curve value is constant between this key and the next
		ConstantNext = 0x00000100		// Curve value is constant, with next key's value
	};
}

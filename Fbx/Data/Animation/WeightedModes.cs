using System;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	///
	/// Flags to determine how a keyframe's tangents are weighted.
	/// </summary>
	[Flags]
	public enum WeightedModes
	{
		WeightedNone = 0x00000000,						// Tangent has default weights of 0.333; we define this state as not weighted.
		WeightedRight = 0x01000000,						// Right tangent is weighted.
		WeightedNextLeft = 0x02000000,					// Left tangent is weighted.
		WeightedAll = WeightedRight|WeightedNextLeft	// Both left and right tangents are weighted.
	};
}

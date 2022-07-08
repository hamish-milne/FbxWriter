using System;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	///
	/// Flags for how tangents should be visualized.
	/// </summary>
	[Flags]
	public enum TangentVisibilities
	{
		TangentShowNone = 0x00000000,							// No tangent is visible.
		TangentShowLeft = 0x00100000,							// Left tangent is visible.
		TangentShowRight = 0x00200000,							// Right tangent is visible.
		TangentShowBoth = TangentShowLeft|TangentShowRight		// Both left and right tangents are visible.
	};
}

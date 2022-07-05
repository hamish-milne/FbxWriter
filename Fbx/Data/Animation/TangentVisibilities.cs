using System;

namespace Fbx.Data.Animation
{
	[Flags]
	public enum TangentVisibilities
	{
		TangentShowNone = 0x00000000,							// No tangent is visible.
		TangentShowLeft = 0x00100000,							// Left tangent is visible.
		TangentShowRight = 0x00200000,							// Right tangent is visible.
		TangentShowBoth = TangentShowLeft|TangentShowRight		// Both left and right tangents are visible.
	};
}

using System;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	/// 
	/// The attribute flags that keys can have.
	/// </summary>
	[Flags]
	public enum TangentModes
	{
		Auto = 0x00000100, // Auto key (spline cardinal).
		TCB = 0x00000200, // Spline TCB (Tension, Continuity, Bias)
		User = 0x00000400, // Next slope at the left equal to slope at the right.
		GenericBreak = 0x00000800, // Independent left and right slopes.
		Break = GenericBreak|User, // Independent left and right slopes, with next slope at the left equal to slope at the right.
		AutoBreak = GenericBreak|Auto, // Independent left and right slopes, with auto key.
		GenericClamp = 0x00001000, // Clamp: key should be flat if next or previous key has the same value (overrides tangent mode).
		GenericTimeIndependent = 0x00002000, // Time independent tangent (overrides tangent mode).
		GenericClampProgressive = 0x00004000|GenericTimeIndependent, // Clamp progressive: key should be flat if tangent control point is outside [next-previous key] range (overrides tangent mode).
	}
}

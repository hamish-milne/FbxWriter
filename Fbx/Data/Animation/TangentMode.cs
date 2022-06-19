using System;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// The attribute flags that keys can have.
	/// 
	/// NOTE: Taken from the Autodesk FBX SDK documentation:
	/// https://help.autodesk.com/cloudhelp/2017/ENU/FBX-Developer-Help/cpp_ref/class_fbx_anim_curve_def.html#acda0d69b7f6c9aef2886b91230b780e2
	/// </summary>
	[Flags]
	public enum TangentMode
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
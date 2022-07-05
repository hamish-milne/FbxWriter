using System;

namespace Fbx.Data.Animation
{
	[Flags]
	public enum ConstantModes
	{
		ConstantStandard = 0x00000000,	// Curve value is constant between this key and the next
		ConstantNext = 0x00000100		// Curve value is constant, with next key's value
	};
}

using System;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	///
	/// FbxAnimCurveKey data indices for cubic interpolation tangent information.
	///
	/// NOTE: I think that means that the KeyAttrDataFloat field of AnimationCurve objects is an array of integers,
	/// and these specify what data is found where in the array. It's a little bit cryptic.
	/// </summary>
	public enum TangentDataIndices
	{
		RightSlope = 0,			// Index of the right derivative, User and Break tangent mode (data are float).
		NextLeftSlope = 1,		// Index of the left derivative for the next key, User and Break tangent mode.
		Weights = 2,			// Start index of weight values, User and Break tangent break mode (data are FbxInt16 tokens from weight and converted to float).
		RightWeight = 2,		// Index of weight on right tangent, User and Break tangent break mode.
		NextLeftWeight = 3,		// Index of weight on next key's left tangent, User and Break tangent break mode.
		Velocity = 4,			// Start index of velocity values, Velocity mode
		RightVelocity = 4,		// Index of velocity on right tangent, Velocity mode
		NextLeftVelocity = 5,	// Index of velocity on next key's left tangent, Velocity mode
		TCBTension = 0,			// Index of Tension, TCB tangent mode (data are floats).
		TCBContinuity = 1,		// Index of Continuity, TCB tangent mode.
		TCBBias = 2				// Index of Bias, TCB tangent mode.
	}
}

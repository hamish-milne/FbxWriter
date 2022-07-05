using Fbx.Data.Times;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// A single key in an animated curve.
	/// </summary>
	public class Key
	{
		private FbxTime time;
		public FbxTime Time => time;

		private float value;
		public float Value => value;
		
		private InterpolationType interpolationType;

		private TangentModes tangentMode;
		private ConstantModes constantMode;
		private TangentVisibilities tangentVisibility;
		private WeightedModes weightedMode;
		private VelocityModes velocityMode;

		public int AttributeFlags
		{
			get
			{
				int flags = (int)interpolationType;
				
				if (interpolationType == InterpolationType.Constant)
					flags |= (int)constantMode;
				else
					flags |= (int)tangentMode;
				
				flags |= (int)tangentVisibility;
				flags |= (int)weightedMode;
				flags |= (int)velocityMode;
				return flags;
			}
		}

		// It's like constructors, but better! Because they're named.
		public static Key Linear(FbxTime time, float value)
		{
			Key key = new Key
			{
				time = time,
				value = value,
				interpolationType = InterpolationType.Linear,
				tangentMode = TangentModes.User,
			};
			return key;
		}
		
		public static Key Constant(FbxTime time, float value, ConstantModes constantMode = ConstantModes.ConstantStandard)
		{
			Key key = new Key
			{
				time = time,
				value = value,
				interpolationType = InterpolationType.Constant,
				constantMode = constantMode,
			};
			return key;
		}

		public static Key Cubic(
			FbxTime time, float value, TangentModes tangentMode = TangentModes.Auto,
			WeightedModes weightedMode = WeightedModes.WeightedNone,
			VelocityModes velocityMode = VelocityModes.None,
			TangentVisibilities tangentVisibility = TangentVisibilities.TangentShowNone)
		{
			Key key = new Key
			{
				time = time,
				value = value,
				interpolationType = InterpolationType.Cubic,
				tangentMode = tangentMode,
				weightedMode = weightedMode,
				velocityMode = velocityMode,
				tangentVisibility = tangentVisibility
			};
			return key;
		}
	}
}

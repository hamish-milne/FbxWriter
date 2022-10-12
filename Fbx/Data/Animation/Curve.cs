using System.Collections;
using System.Collections.Generic;
using Fbx.Data.Times;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// Contains the animated values of a specific property expressed as a curve over time.
	/// </summary>
	public class Curve : IEnumerable<Key>
	{
		private readonly FbxId id;
		public FbxId Id => id;
		
		private readonly Joint joint;
		public Joint Joint => joint;

		private AnimatablePropertyBase property;
		public AnimatablePropertyBase Property => property;
		
		private Components component;
		public Components Component => component;

		private List<Key> keys = new List<Key>();

		public int Count => keys.Count;

		public Curve(AnimatablePropertyBase property, Components component)
		{
			id = FbxId.GetNewId();
			this.property = property;
			this.component = component;
		}

		// Linear
		public void Add(FbxTime time, float value) 
		{
			keys.Add(Key.Linear(time, value));
		}
		// Constant
		public void Add(FbxTime time, float value, ConstantModes constantMode = ConstantModes.ConstantStandard)
		{
			keys.Add(Key.Constant(time, value, constantMode));
		}
		
		// Cubic
		public void Add(FbxTime time, float value, TangentModes tangentMode,
			WeightedModes weightedMode = WeightedModes.WeightedNone,
			VelocityModes velocityMode = VelocityModes.None,
			TangentVisibilities tangentVisibility = TangentVisibilities.TangentShowNone)
		{
			keys.Add(Key.Cubic(time, value, tangentMode, weightedMode, velocityMode, tangentVisibility));
		}

		public IEnumerator<Key> GetEnumerator()
		{
			return keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Key this[int index] => keys[index];
	}
}

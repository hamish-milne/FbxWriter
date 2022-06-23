using System;
using Fbx.Data.Animation;

namespace Fbx.Data
{
	/// <summary>
	/// Wraps a property that can be animated.
	/// </summary>
	/// <typeparam name="ValueType">Type of value.</typeparam>
	public class AnimatableProperty<ValueType> : AnimatablePropertyBase
	{
		private ValueType value;
		public ValueType Value => value;

		private FbxNode animationCurveNode;
		
		private readonly AnimatablePropertyTypes animatablePropertyType;
		public override AnimatablePropertyTypes AnimatablePropertyType => animatablePropertyType;

		public AnimatableProperty(AnimatablePropertyTypes animatablePropertyType, ValueType value)
		{
			this.value = value;
			this.animatablePropertyType = animatablePropertyType;
		}

		public static implicit operator ValueType(AnimatableProperty<ValueType> animatableProperty)
		{
			return animatableProperty.Value;
		}

		public override Type GetValueType()
		{
			return typeof(ValueType);
		}

		public override object GetValueRaw()
		{
			return Value;
		}
	}
}

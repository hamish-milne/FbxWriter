using System;

namespace Fbx.Data
{
	/// <summary>
	/// Wraps a property that can be animated.
	/// </summary>
	/// <typeparam name="ValueType">Type of value.</typeparam>
	public class AnimatableProperty<ValueType> : AnimatablePropertyBase
	{
		private ValueType value;
		public ValueType Value
		{
			get => value;
			set => this.value = value;
		}

		private FbxNode animationCurveNode;

		public AnimatableProperty(string nodeName, string propertyName, ValueType value)
			: base(nodeName, propertyName)
		{
			this.value = value;
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

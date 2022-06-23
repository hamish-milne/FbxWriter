using System;
using Fbx.Data.Animation;

namespace Fbx.Data
{
	public abstract class AnimatablePropertyBase
	{
		private FbxId animationCurveNodeId;
		public FbxId AnimationCurveNodeId
		{
			get => animationCurveNodeId;
			set => animationCurveNodeId = value;
		}

		public abstract AnimatablePropertyTypes AnimatablePropertyType { get; }

		public abstract Type GetValueType();
		public abstract object GetValueRaw();
	}
}

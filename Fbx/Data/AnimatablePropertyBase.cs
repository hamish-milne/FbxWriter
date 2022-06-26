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
		
		private readonly string nodeName;
		public string NodeName => nodeName;

		private readonly string propertyName;
		public string PropertyName => propertyName;
		
		private IAnimatablePropertyOwner owner;
		public IAnimatablePropertyOwner Owner => owner;

		public abstract AnimatablePropertyTypes AnimatablePropertyType { get; }

		public abstract Type GetValueType();
		public abstract object GetValueRaw();

		protected AnimatablePropertyBase(string nodeName, string propertyName)
		{
			this.nodeName = nodeName;
			this.propertyName = propertyName;
		}

		public void Initialize(IAnimatablePropertyOwner owner)
		{
			this.owner = owner;
		}
	}
}

using System.Collections;
using System.Collections.Generic;

namespace Fbx.Data.Animation
{
	/// <summary>
	/// Contains the animated values of a specific property expressed as a curve over time.
	/// </summary>
	public class Curve : IEnumerable<Key>
	{
		private readonly FbxNodeId id;
		public FbxNodeId Id => id;
		
		private readonly Joint joint;
		public Joint Joint => joint;

		private CurveAttributes attribute;
		public CurveAttributes Attribute => attribute;

		private List<Key> keys = new List<Key>();

		public Curve(Joint joint, CurveAttributes attribute)
		{
			id = FbxNodeId.GetNewId();
			this.joint = joint;
			this.attribute = attribute;
		}

		public void Add(float time, float value)
		{
			Key key = new Key(time, value);
			keys.Add(key);
		}

		public IEnumerator<Key> GetEnumerator()
		{
			return keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

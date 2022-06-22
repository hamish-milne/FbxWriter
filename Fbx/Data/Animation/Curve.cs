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

		private CurveChannels channel;
		public CurveChannels Channel => channel;

		private List<Key> keys = new List<Key>();

		public int Count => keys.Count;

		public Curve(Joint joint, CurveChannels channel)
		{
			id = FbxId.GetNewId();
			this.joint = joint;
			this.channel = channel;
		}

		public void Add(FbxTime time, float value, TangentModes tangentMode)
		{
			keys.Add(new Key(time, value, tangentMode));
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

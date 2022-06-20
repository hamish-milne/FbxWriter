namespace Fbx.Data.Animation
{
	/// <summary>
	/// A single key in an animated curve.
	/// </summary>
	public class Key
	{
		private readonly long time;
		public long Time => time;

		private readonly float value;
		public float Value => value;
		
		private readonly TangentModes tangentMode;
		public TangentModes TangentMode => tangentMode;

		public Key(long time, float value, TangentModes tangentMode)
		{
			this.time = time;
			this.value = value;
			this.tangentMode = tangentMode;
		}
	}
}

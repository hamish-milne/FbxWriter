namespace Fbx.Data.Animation
{
	/// <summary>
	/// A single key in an animated curve.
	/// </summary>
	public class Key
	{
		private readonly float time;
		public float Time => time;

		private readonly float value;
		public float Value => value;

		public Key(float time, float value)
		{
			this.time = time;
			this.value = value;
		}
	}
}

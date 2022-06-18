namespace Fbx.PropertyBlocks
{
	public struct Vector3D
	{
		public static readonly Vector3D Zero = new Vector3D();
		public static readonly Vector3D One = new Vector3D(1, 1, 1);
		
		public float X, Y, Z;

		public Vector3D(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}

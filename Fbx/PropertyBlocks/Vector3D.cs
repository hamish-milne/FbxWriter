using System;

namespace Fbx.PropertyBlocks
{
	public struct Vector3D : IEquatable<Vector3D>
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

		private static bool Approximately(float a, float b)
		{
			return Math.Abs(a - b) < float.Epsilon;
		}

		/// <summary>
		/// Whether the value of this Vector3D is equal to the other.
		/// </summary>
		/// <param name="other">Other Vector3D to compare against.</param>
		/// <returns>True if all components are equal, false otherwise.</returns>
		public bool Equals(Vector3D other)
		{
			return Approximately(X, other.X) && Approximately(Y, other.Y) && Approximately(Z, other.Z);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			return obj is Vector3D other && Equals(other);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = X.GetHashCode();
				hashCode = (hashCode * 397) ^ Y.GetHashCode();
				hashCode = (hashCode * 397) ^ Z.GetHashCode();
				return hashCode;
			}
		}
		
		public static bool operator ==(Vector3D lhs, Vector3D rhs) => lhs.Equals(rhs);

		public static bool operator !=(Vector3D lhs, Vector3D rhs) => !(lhs == rhs);
	}
}

using System;

namespace Fbx.PropertyBlocks
{
	/// <summary>
	/// A color with RGB components.
	/// </summary>
	public struct ColorRGB
	{
		private float r;
		public float R
		{
			get => r;
			set => r = Math.Max(value, 0.0f);
		}

		private float g;
		public float G
		{
			get => g;
			set => g = Math.Max(value, 0.0f);
		}

		private float b;
		public float B
		{
			get => b;
			set => b = Math.Max(value, 0.0f);
		}

		public ColorRGB(float r, float g, float b)
		{
			this.r = Math.Max(0.0f, r);
			this.g = Math.Max(0.0f, g);
			this.b = Math.Max(0.0f, b);
		}
	}
}

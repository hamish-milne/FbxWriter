namespace Fbx.Data.Animation
{
	/// <summary>
	/// The known allowed components that can be animated. I suspect that when you animate a camera you get a bunch of
	/// other options like FOV, near clip plane, far clip plane...
	/// These names are important! Don't "fix" the case, they are written exactly like this.
	/// </summary>
	public enum Components
	{
		filmboxTypeID,
		X,
		Y,
		Z,
	}
}

namespace Fbx.Data
{
	/// <summary>
	/// Owner of an animatable property.
	/// </summary>
	public interface IAnimatablePropertyOwner
	{
		FbxId Id { get; }
	}
}
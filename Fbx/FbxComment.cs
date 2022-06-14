namespace Fbx
{
	/// <summary>
	/// Empty node that can be inserted amidst nodes for organizing the ASCII file. Optionally has a divider, too.
	/// </summary>
	public class FbxComment : FbxNode
	{
		public bool HasDivider { get; set; }
	}
}

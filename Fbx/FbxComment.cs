namespace Fbx
{
	/// <summary>
	/// Text node that can be inserted amidst nodes for organizing the ASCII file. Optionally has a divider, too.
	/// </summary>
	public class FbxComment : FbxNode
	{
		public bool HasDivider { get; set; }
		public bool HasSpace { get; set; }
	}
}

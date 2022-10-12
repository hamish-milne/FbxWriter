namespace Fbx
{
	/// <summary>
	/// Text node that can be inserted amidst nodes for organizing the ASCII file. Optionally has a divider, too.
	/// </summary>
	public class FbxComment : FbxNode
	{
		public CommentTypes Type { get; set; }
	}
}

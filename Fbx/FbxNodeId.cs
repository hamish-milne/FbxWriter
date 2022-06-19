namespace Fbx
{
	/// <summary>
	/// Generates a unique ID for an FBX node.
	/// </summary>
	public class FbxNodeId
	{
		private const long InitialId = 2000000000000;

		private long id;

		private static long lastId = InitialId;

		/// <summary>
		/// Converts the node ID to a string.
		/// </summary>
		/// <returns>The 13-digit long unique string representing a node.</returns>
		public override string ToString()
		{
			return id.ToString("0000000000000");
		}

		/// <summary>
		/// Generates a new unique identifier for an FBX node.
		/// </summary>
		public static FbxNodeId GetNewId()
		{
			return new FbxNodeId { id = lastId++ };
		}
	}
}

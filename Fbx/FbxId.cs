namespace Fbx
{
	/// <summary>
	/// Generates a unique ID for an FBX node or property.
	/// </summary>
	public class FbxId
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
			return id.ToString();
		}

		/// <summary>
		/// Generates a new unique identifier for an FBX node.
		/// </summary>
		public static FbxId GetNewId()
		{
			return new FbxId { id = lastId++ };
		}
	}
}

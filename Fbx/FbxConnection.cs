namespace Fbx
{
	/// <summary>
	/// Represents a connection between objects or properties.
	/// </summary>
	public class FbxConnection
	{
		private ConnectionTypes connectionType;
		public ConnectionTypes ConnectionType => connectionType;

		private FbxId fromId;
		public FbxId FromId => fromId;
		
		private string fromType;
		public string FromType => fromType;
		
		private string fromName;
		public string FromName => fromName;
		
		private FbxId toId;
		public FbxId ToId => toId;
		
		private string toType;
		public string ToType => toType;
		
		private string toName;
		public string ToName => toName;

		public FbxConnection(ConnectionTypes connectionType,
			FbxId fromId, string fromType, string fromName,
			FbxId toId, string toType, string toName)
		{
			this.connectionType = connectionType;
			this.fromId = fromId;
			this.fromType = fromType;
			this.fromName = fromName;
			this.toId = toId;
			this.toType = toType;
			this.toName = toName;
		}
	}
}

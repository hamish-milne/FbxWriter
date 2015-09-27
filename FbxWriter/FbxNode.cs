using System.Collections.Generic;

namespace Fbx
{
	public class FbxNode
	{
		public string Name { get; set; }

		public List<object> Properties { get; } = new List<object>();

		public List<FbxNode> Nodes { get; } = new List<FbxNode>();

		public bool IsEmpty => string.IsNullOrEmpty(Name) && Properties.Count == 0 && Nodes.Count == 0;
	}
}

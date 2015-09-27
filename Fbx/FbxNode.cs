using System.Collections.Generic;

namespace Fbx
{
	public class FbxNode
	{
		public string Name { get; set; }

		public List<object> Properties { get; } = new List<object>();

		public List<FbxNode> Nodes { get; } = new List<FbxNode>();

		public bool IsEmpty => string.IsNullOrEmpty(Name) && Properties.Count == 0 && Nodes.Count == 0;

		public FbxNode this[string name] { get { return Nodes.Find(n => n != null && n.Name == name); } }

		public FbxNode GetRelative(string path)
		{
			var tokens = path.Split('/');
			var n = this;
			foreach (var t in tokens)
			{
				if(t == "")
					continue;
				n = n[t];
				if(n == null)
					break;
			}
			return n;
		}
	}
}

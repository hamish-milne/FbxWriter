using Fbx.PropertyBlocks;

namespace Fbx.Data
{
	/// <summary>
	/// Represents a single joint. The intended workflow is that you create a joint hierarchy and animation data,
	/// add it to an FbxTemplate, then write that to an FBX. This allows you to work on a simple and intuitive level
	/// which is translated to the complicated FBX syntax under the hood. 
	/// </summary>
	public class Joint
	{
		private string name;
		public string Name => name;

		private long id;
		public long Id => id;

		private long attributesNodeId;
		public long AttributesNodeId => attributesNodeId;

		private long animCurveNodeId;
		public long AnimCurveNodeId => animCurveNodeId;

		private Vector3D position;
		public Vector3D Position => position;

		/// <summary>
		/// Creates a new joint.
		/// </summary>
		/// <param name="name">Name of the joint.</param>
		/// <param name="id">ID of the joint node. TODO: Auto-generate this.</param>
		/// <param name="attributesNodeId">ID of the joint's attribute node. TODO: Auto-generate this.</param>
		/// <param name="animCurveNodeId">ID of the joint's anim curve node. TODO: Auto-generate this.</param>
		/// <param name="position">Start position. TODO: Support animations.</param>
		public Joint(string name, long id, long attributesNodeId, long animCurveNodeId, Vector3D position)
		{
			this.name = name;
			this.id = id;
			this.attributesNodeId = attributesNodeId;
			this.animCurveNodeId = animCurveNodeId;
			this.position = position;
		}
	}
}

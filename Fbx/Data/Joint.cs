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

		private FbxId id;
		public FbxId Id => id;

		private FbxId attributesNodeId;
		public FbxId AttributesNodeId => attributesNodeId;

		private FbxId animCurveNodeId;
		public FbxId AnimCurveNodeId => animCurveNodeId;

		private Vector3D translation;
		public Vector3D Translation => translation;
		
		private Vector3D rotation;
		public Vector3D Rotation => rotation;
		
		private Vector3D scaling;
		public Vector3D Scaling => scaling;

		private Joint parent;
		public Joint Parent
		{
			get => parent;
		}

		/// <summary>
		/// Creates a new joint.
		/// </summary>
		/// <param name="name">Name of the joint.</param>
		/// <param name="translation">Start position. TODO: Support animations.</param>
		/// <param name="rotation">Start rotation.</param>
		/// <param name="scaling">Start scaling.</param>
		public Joint(string name, Vector3D translation, Vector3D rotation, Vector3D scaling, Joint parent = null)
		{
			this.name = name;
			id = FbxId.GetNewId();
			attributesNodeId = FbxId.GetNewId();
			animCurveNodeId = FbxId.GetNewId();
			this.translation = translation;
			this.rotation = rotation;
			this.scaling = scaling;
			this.parent = parent;
		}

		/// <summary>
		/// Creates a new joint.
		/// </summary>
		/// <param name="name">Name of the joint.</param>
		/// <param name="translation">Start position. TODO: Support animations.</param>
		public Joint(string name, Vector3D translation, Vector3D rotation)
			: this(name, translation, rotation, Vector3D.One)
		{
		}

		/// <summary>
		/// Creates a new joint.
		/// </summary>
		/// <param name="name">Name of the joint.</param>
		/// <param name="translation">Start position. TODO: Support animations.</param>
		public Joint(string name, Vector3D translation)
			: this(name, translation, Vector3D.Zero)
		{
		}
	}
}

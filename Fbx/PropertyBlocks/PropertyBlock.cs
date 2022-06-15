using System;

namespace Fbx.PropertyBlocks
{
	/// <summary>
	/// Utility for quickly creating a block of properties using the standard format.
	/// </summary>
	public class PropertyBlock
	{
		private const string PropertyName = "P";
		private const string DateTimeFormat = "dd'/'MM'/'yyyy HH:mm:ss.fff";
		
		private FbxNodeList root;

		/// <summary>
		/// Creates a new property block as a child of the specified node.
		/// </summary>
		/// <param name="nodeToAddTo">Node to which to add a new property block/</param>
		public PropertyBlock(FbxNodeList nodeToAddTo)
		{
			root = nodeToAddTo.Add("Properties70");
		}
		
		public void AddObject(string name, object value)
		{
			root.Add(PropertyName, name, "object", "", "", value);
		}
		
		public void AddCompound(string name)
		{
			// TODO: Do something clever here so we can add properties to this compound object and have it be
			// formatted automatically? Maybe an in-between object?
			root.Add(PropertyName, name, "Compound", "", "");
		}
		
		public void AddString(string name, string value, StringTypes type = StringTypes.Default)
		{
			string typeString = type == StringTypes.Default ? "" : type.ToString();
			root.Add(PropertyName, name, "KString", typeString, "", value);
		}
		
		public void AddDateTime(string name, DateTime value)
		{
			root.Add(PropertyName, name, "DateTime", "", "", value.ToString(DateTimeFormat));
		}
		
		public void AddTime(string name, DateTime value)
		{
			root.Add(PropertyName, name, "KTime", "Time", "", value.Ticks);
		}
		
		public void AddInteger(string name, int value)
		{
			root.Add(PropertyName, name, "int", "Integer", "", value);
		}
		
		public void AddDouble(string name, double value)
		{
			root.Add(PropertyName, name, "double", "Number", "", value);
		}

		public void AddEnum(string name, int value)
		{
			root.Add(PropertyName, name, "enum", "", "", value);
		}
		
		public void AddEnum<EnumType>(string name, EnumType value)
			where EnumType : Enum
		{
			AddEnum(name, (int)(object)value);
		}
		
		public void AddColorRGB(string name, ColorRGB value)
		{
			root.Add(PropertyName, name, "ColorRGB", "Color", "", value.R, value.G, value.B);
		}
	}
}

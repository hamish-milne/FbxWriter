using System;
using Fbx.Data.Times;

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
		
		public CompoundProperty AddCompound(string name)
		{
			root.Add(PropertyName, name, "Compound", "", "");
			return new CompoundProperty(this, name);
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
		
		public void AddTime(string name, FbxTime value)
		{
			root.Add(PropertyName, name, "KTime", "Time", "", value);
		}

		public void AddBool(string name, bool value)
		{
			root.Add(PropertyName, name, "bool", "", "", value ? 1 : 0);
		}
		
		public void AddInteger(string name, int value)
		{
			root.Add(PropertyName, name, "int", "Integer", "", value);
		}
		
		public void AddNumber(string name, float value)
		{
			root.Add(PropertyName, name, "Number", "", "A", value);
		}
		
		public void AddDouble(string name, double value, DoubleTypes type = DoubleTypes.Default)
		{
			string typeString = type == DoubleTypes.Default ? "" : type.ToString();
			root.Add(PropertyName, name, "double", "Number", typeString, value);
		}

		public void AddULongLong(string name, ulong value)
		{
			root.Add(PropertyName, name, "ULongLong", "", "", value);
		}

		public void AddShort(string name, short value, ShortTypes type = ShortTypes.Default)
		{
			string typeString;
			switch (type)
			{
				default: typeString = "A"; break;
				case ShortTypes.APlusUH: typeString = "A+UH"; break;
			}
			
			// If someone could explain to me what on earth this is for, that would be great.
			if (type == ShortTypes.APlusUH)
				root.Add(PropertyName, name, "Short", "", typeString, value, value, value);
			else
				root.Add(PropertyName, name, "Short", "", typeString, value);
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

		public void AddVector3D(string name, Vector3D value)
		{
			root.Add(PropertyName, name, "Vector3D", "Vector", "", value.X, value.Y, value.Z);
		}
		
		// These are for transformations.
		public void AddLclTranslation(string name, Vector3D value)
		{
			root.Add(PropertyName, name, "Lcl Translation", "", "A", value.X, value.Y, value.Z);
		}
		
		public void AddLclRotation(string name, Vector3D value)
		{
			root.Add(PropertyName, name, "Lcl Rotation", "", "A", value.X, value.Y, value.Z);
		}
		
		public void AddLclScaling(string name, Vector3D value)
		{
			root.Add(PropertyName, name, "Lcl Scaling", "", "A", value.X, value.Y, value.Z);
		}
		
		public void AddVisibility(string name, bool value)
		{
			root.Add(PropertyName, name, "Visibility", "", "A", value);
		}
		
		public void AddVisibilityInheritance(string name, bool value)
		{
			root.Add(PropertyName, name, "Visibility Inheritance", "", "", value);
		}
	}
}

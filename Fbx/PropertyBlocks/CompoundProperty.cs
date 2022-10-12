using System;

namespace Fbx.PropertyBlocks
{
	/// <summary>
	/// A 'compound' property is an object represented by multiple properties tied together with a prefix. 
	/// </summary>
	public class CompoundProperty
	{
		private const char Separator = '|';
		
		private PropertyBlock propertyBlock;
		private string prefix;

		public CompoundProperty(PropertyBlock propertyBlock, string name)
		{
			this.propertyBlock = propertyBlock;
			prefix = name + Separator;
		}

		public void AddObject(string name, object value)
		{
			propertyBlock.AddObject(prefix + name, value);
		}

		public void AddString(string name, string value, StringTypes type = StringTypes.Default)
		{
			propertyBlock.AddString(prefix + name, value, type);
		}

		public void AddDateTime(string name, DateTime value)
		{
			propertyBlock.AddDateTime(prefix + name, value);
		}

		public void AddTime(string name, DateTime value)
		{
			propertyBlock.AddTime(prefix + name, value);
		}

		public void AddInteger(string name, int value)
		{
			propertyBlock.AddInteger(prefix + name, value);
		}

		public void AddDouble(string name, double value)
		{
			propertyBlock.AddDouble(prefix + name, value);
		}

		public void AddEnum(string name, int value)
		{
			propertyBlock.AddEnum(prefix + name, value);
		}

		public void AddEnum<EnumType>(string name, EnumType value) where EnumType : Enum
		{
			propertyBlock.AddEnum(prefix + name, value);
		}

		public void AddColorRGB(string name, ColorRGB value)
		{
			propertyBlock.AddColorRGB(prefix + name, value);
		}
	}
}

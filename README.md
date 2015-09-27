# FBX manipulation for .NET

- Read FBX binary files (Done)
- Read FBX ASCII files (TODO)
- Write **fully compliant** FBX binary files (Done)
- Write FBX ASCII files (Preview only)
- Format detection (TODO)
- Store and manipulate raw FBX object data (Done)
- Higher level processing of FBX nodes (TODO)

```
using Fbx;

class FbxExample
{
	static void Main(string[] args)
	{
		// Read a file
		var documentNode = FbxUtil.ReadBinary("MyModel.fbx");
		// Update a property
		documentNode["Creator"].Properties[0] = "My Application";
		// Preview the file in the console
		var writer = new FbxAsciiWriter(Console.OpenStandardOutput());
		writer.Write(documentNode);
		// Write the updated binary
		FbxUtil.WriteBinary(documentNode, "MyModel_patched.fbx");
	}
}
```

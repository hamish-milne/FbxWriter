# FBX manipulation for .NET (expanded to support joint hierarchies & animations)

## What's added in this fork?
- Created an `FbxBuilder` utility to allow you to quickly create an FBX file by focusing on the data you want to write instead of having to re-create the entire FBX file format yourself
- Added high-level classes for creating Joints and Animation Curves.
- Added `FBXTime` wrapper to convert from frames or seconds to the internal FBX format
- Added basic support for unique ID generation (could be improved, but it works)
- Improved syntax for adding new nodes and property blocks
- Expanded `FBXWriter` to support inline comments, headers and line breaks
- Expanded `FBXWriter` to support `FBXTime` and various other data types

Here's an example of how to use the new `FbxBuilder`:

```csharp
FbxBuilder builder = new FbxBuilder(@"C:\Output\Test.fbx", 30);

// Add a basic joint.
Joint joint1 = builder.AddJoint(new Joint("joint1", new Vector3D(0, 0, 100)));

// Add an animation for joint1's position attribute X-coordinate from frame 1 to 30.
Curve joint1PositionCurve = new Curve(joint1.Translation, Components.X)
{
    { 1, 0.0f },
    { 30, -100.0f },
};
builder.AddCurve(joint1PositionCurve);

builder.Write();
```

### Known issues
- `FbxId` generates ID's that are unique to the file, but they're not globally unique. Not sure if this is problematic. Works fine for me.
- Custom tangent slopes, weights and velocities are not currently supported. This leaves the following possible tangents:
    - Constant
    - Linear
    - Cubic with flat tangents, or with automatic tangents.

## Original docs
- Read FBX binary files (**Done**)
- Read FBX ASCII files (**Done**)
- Write **fully compliant** FBX binary files (**Done**)
- Write FBX ASCII files (**Done**)
- Format detection (TODO)
- Store and manipulate raw FBX object data (**Done**)
- Higher level processing of FBX nodes (TODO)
- Optional integration with DotNetZip for more efficient compression (TODO)

```csharp
using Fbx;

class FbxExample
{
	static void Main(string[] args)
	{
		// Read a file
		var documentNode = FbxIO.ReadBinary("MyModel.fbx");
		
		// Update a property
		documentNode["Creator"].Value = "My Application";
		
		// Preview the file in the console
		var writer = new FbxAsciiWriter(Console.OpenStandardOutput());
		writer.Write(documentNode);
		
		// Write the updated binary
		FbxIO.WriteBinary(documentNode, "MyModel_patched.fbx");
	}
}
```

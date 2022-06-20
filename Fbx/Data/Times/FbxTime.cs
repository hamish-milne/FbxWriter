namespace Fbx.Data.Times
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	///
	/// Class to encapsulate time units.
	/// FbxTime can measure time in hour, minute, second, frame, field, residual and also combination of these units.
	/// It is recommended to use FbxTime for all time related operations.
	/// For example, currently it is used in FbxGlobalSettings, FbxGlobalTimeSettings, FbxCache,
	/// all curve filters and all animation-related classes, etc.
	/// FbxTime is just used to represent a moment, to represent a period of time, FbxTimeSpan should be used.
	/// </summary>
	public struct FbxTime
	{
		/// <summary>
		/// What seems to be the interval between frames as exported from a 30FPS file.
		/// </summary>
		private const long IntervalPerThirtiethOfSecond = 1539538600;
		
		private long time;
		
		/// <summary>
		/// The time value as specified in the strange custom FBX format.
		/// </summary>
		public long TimeInInternalFormat
		{
			get => time;
			set => time = value;
		}

		/// <summary>
		/// Initializes the time based on the frame count. Note that the frame count should start with 1!
		/// </summary>
		/// <param name="frame">Frame count, with the first one being 1.</param>
		public void SetViaFrame(long frame)
		{
			// Had to reverse engineer the format because it's not documented anywhere, lol
			// NOTE: This currently assumes a 30 FPS file. If you want to support other formats, you need to update this
			// conversion to work for every type of TimeMode. Have fun with that...
			time = frame * IntervalPerThirtiethOfSecond;
		}

		/// <summary>
		/// Converts a frame index to an FbxTime object.
		/// </summary>
		/// <param name="frame">The frame index.</param>
		/// <returns>An FbxTime object representing the specified frame index.</returns>
		public static implicit operator FbxTime(long frame)
		{
			FbxTime fbxTime = new FbxTime();
			fbxTime.SetViaFrame(frame);
			return fbxTime;
		}

		public override string ToString()
		{
			return time.ToString();
		}
	}
}

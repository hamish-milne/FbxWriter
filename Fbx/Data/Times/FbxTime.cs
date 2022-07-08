using System;

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
		private static double frameRate = 30;
		public static double FrameRate
		{
			get => frameRate;
			set => frameRate = value;
		}

		/// <summary>
		/// What seems to be the interval between frames as exported from a 30FPS file.
		/// </summary>
		private static double IntervalPerSecond = 46186158000;
		private static double IntervalPerFrame => IntervalPerSecond / FrameRate;
		
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
		public static FbxTime Frames(long frame)
		{
			return new FbxTime { time = (long)(frame * IntervalPerFrame) };
		}
		
		public static FbxTime Seconds(double seconds)
		{
			return new FbxTime { time = (long)(seconds * IntervalPerSecond) };
		}

		/// <summary>
		/// Converts a frame index to an FbxTime object.
		/// </summary>
		/// <param name="frames">The frame index.</param>
		/// <returns>An FbxTime object representing the specified frame index.</returns>
		public static implicit operator FbxTime(long frames)
		{
			return Frames(frames);
		}
		
		/// <summary>
		/// Converts a DateTime to an FbxTime object.
		/// </summary>
		/// <param name="time">The time to convert.</param>
		/// <returns>An FbxTime object representing the specified date and time.</returns>
		public static implicit operator FbxTime(DateTime time)
		{
			double ticksPerSecond = 10000000; // 10 million ticks per second.
			double seconds = time.Ticks / ticksPerSecond;
			return Seconds(seconds);
		}

		public override string ToString()
		{
			return time.ToString();
		}
	}
}

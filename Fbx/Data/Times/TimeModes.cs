namespace Fbx.Data.Times
{
	/// <summary>
	/// Taken from the Autodesk FBX SDK and reformatted. All rights reserved.
	/// 
	/// NTSCDropFrame is used for broadcasting operations where clock time must be (almost) in sync with time code. To
	/// bring back color NTSC time code with clock time, this mode drops 2 frames per minute except for every 10
	/// minutes (00, 10, 20, 30, 40, 50). 108 frames are dropped per hour. Over 24 hours the error is 2 frames and 1/4
	/// of a frame. A time-code of 01:00:03:18 equals a clock time of 01:00:00:00
	/// 
	/// NTSCFullFrame represents a time address and therefore is NOT IN SYNC with clock time.
	/// A time code of 01:00:00:00 equals a clock time of 01:00:03:18.
	/// 
	/// DefaultMode		
	/// Frames120			120 frames/s
	/// Frames100			100 frames/s
	/// Frames60          60 frames/s
	/// Frames50          50 frames/s
	/// Frames48          48 frame/s
	/// Frames30          30 frames/s (black and white NTSC)
	/// Frames30Drop		30 frames/s (use when display in frame is selected, equivalent to NTSC drop)
	/// NTSCDropFrame		~29.97 frames/s drop color NTSC
	/// NTSCFullFrame		~29.97 frames/s color NTSC
	/// PAL				25 frames/s	PAL/SECAM
	/// Frames24			24 frames/s Film/Cinema
	/// Frames1000		1000 milli/s (use for date time)
	/// FilmFullFrame		~23.976 frames/s
	/// Custom            Custom frame rate value
	/// Frames96			96 frames/s
	/// Frames72			72 frames/s
	/// Frames59dot94		~59.94 frames/s
	/// Frames119dot88	~119.88 frames/s
	/// ModesCount		Number of time modes
	/// </summary>
	public enum TimeModes
	{
		DefaultMode,
		Frames120,
		Frames100,
		Frames60,
		Frames50,
		Frames48,
		Frames30,
		Frames30Drop,
		NTSCDropFrame,
		NTSCFullFrame,
		PAL,
		Frames24,
		Frames1000,
		FilmFullFrame,
		Custom,
		Frames96,
		Frames72,
		Frames59dot94,
		Frames119dot88,
		ModesCount,
	}
}

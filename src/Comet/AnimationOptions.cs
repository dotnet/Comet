using System;

namespace Comet
{
	[Flags]
	public enum AnimationOptions
	{
		None = 0,
		Repeat = 1,
		CurveEaseIn = 2,
		CurveEaseInOut = 4,
		CurveEaseOut = 8,
	}
}

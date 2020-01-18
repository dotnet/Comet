using System;

namespace Comet.Styles
{
	public class ProgressBarStyle : ViewStyle
	{
		public StyleAwareValue<ControlState, Color> TrackColor { get; set; }
		public StyleAwareValue<ControlState, Color> ProgressColor { get; set; }
	}
}

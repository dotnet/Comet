using System;
namespace System.Maui.Styles
{
	public class SliderStyle : ViewStyle
	{
		public StyleAwareValue<ControlState, Color> TrackColor { get; set; }
		public StyleAwareValue<ControlState, Color> ProgressColor { get; set; }
		public StyleAwareValue<ControlState, Color> ThumbColor { get; set; }
	}
}

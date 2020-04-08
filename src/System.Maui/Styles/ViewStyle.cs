using System;
using System.Maui.Graphics;

namespace System.Maui.Styles
{
	public class ViewStyle
	{
		public StyleAwareValue<ControlState, Color> BackgroundColor { get; set; }

		public StyleAwareValue<ControlState, Shape> Border { get; set; }

		public StyleAwareValue<ControlState, Shadow> Shadow { get; set; }

		public StyleAwareValue<ControlState, Shape> ClipShape { get; set; }

		public StyleAwareValue<ControlState, Shape> Overlay { get; set; }
	}
}

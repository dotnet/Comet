using System.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maui.Styles
{
	public class ButtonStyle : ViewStyle
	{
		public StyleAwareValue<ControlState,Color> TextColor { get; set; }

		public StyleAwareValue<ControlState, FontAttributes> TextFont { get; set; }

		public StyleAwareValue<ControlState, Thickness> Padding { get; set; }

	}
}

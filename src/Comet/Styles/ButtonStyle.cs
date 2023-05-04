using Comet.Graphics;
using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.Styles
{
	public class ButtonStyle : ViewStyle
	{
		public StyleAwareValue<ControlState, Color> TextColor { get; set; }

		public StyleAwareValue<ControlState, Font> TextFont { get; set; }

		public StyleAwareValue<ControlState, Thickness> Padding { get; set; }

	}
}

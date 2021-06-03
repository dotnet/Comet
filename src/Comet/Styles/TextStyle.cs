using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.Styles
{
	public class TextStyle
	{
		public string StyleId { get; set; }
		public StyleAwareValue<ControlState, Font> Font { get; set; }
		public StyleAwareValue<ControlState, Color> Color { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Graphics;
using System.Text;

namespace Comet.Styles
{
	public class TextStyle
	{
		public string StyleId { get; set; }
		public StyleAwareValue<ControlState, FontAttributes> Font { get; set; }
		public StyleAwareValue<ControlState, Color> Color { get; set; }
	}
}

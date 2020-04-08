using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maui.Styles
{
	public class TextStyle
	{
		public string StyleId { get; set; }
		public StyleAwareValue<ControlState, FontAttributes> Font { get; set; }
		public StyleAwareValue<ControlState, Color> Color { get; set; }
	}
}

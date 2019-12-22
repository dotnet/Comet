using Comet.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Styles
{
	public class ButtonStyle
	{
		public StyleAwareValue<ControlState,Color> TextColor { get; set; }

		public StyleAwareValue<ControlState, FontAttributes> TextFont { get; set; }

		public StyleAwareValue<ControlState, Color> BackgroundColor { get; set; }

		public StyleAwareValue<ControlState, Shape> Border { get; set; }

		public StyleAwareValue<ControlState, Shadow> Shadow { get; set; }

		public StyleAwareValue<ControlState, Thickness> Padding { get; set; }

	}
}

﻿using System;

using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet.Styles
{
	public class ViewStyle
	{
		public StyleAwareValue<ControlState, Color> BackgroundColor { get; set; }

		public StyleAwareValue<ControlState, Shape> Border { get; set; }

		public StyleAwareValue<ControlState, Shadow> Shadow { get; set; }

		public StyleAwareValue<ControlState, Shape> ClipShape { get; set; }
	}
}

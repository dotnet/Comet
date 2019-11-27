using System;
using System.Drawing;

namespace Comet
{
	public class Animation
	{
		public double? Duration { get; set; }
		public double? Delay { get; set; }
		public AnimationOptions? Options { get; set; }
		public PointF? TranslateTo { get; set; }
		public double? RotateTo { get; set; }
		public PointF? ScaleTo { get; set; }
	}
}

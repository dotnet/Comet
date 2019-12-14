using System;
using System.Drawing;
using System.Linq;

namespace Comet
{
	public static class RectangleExtensions
	{
		public static bool BoundsContains(this RectangleF rect, PointF point) =>
			point.X >= 0 && point.X <= rect.Width &&
			point.Y >= 0 && point.Y <= rect.Height;

		public static bool Contains(this RectangleF rect, PointF[] points)
			=> points.Any(x => rect.Contains(x));

		public static RectangleF ApplyPadding(this RectangleF rect, Thickness thickness)
		{
			if (thickness == null)
				return rect;
			rect.X += thickness.Left;
			rect.Y += thickness.Top;
			rect.Width -= thickness.HorizontalThickness;
			rect.Height -= thickness.VerticalThickness;

			return rect;
		}

	}
}

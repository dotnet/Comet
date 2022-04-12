using System;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public static class RectangleExtensions
	{
		public static bool BoundsContains(this Rect rect, Point point) =>
			point.X >= 0 && point.X <= rect.Width &&
			point.Y >= 0 && point.Y <= rect.Height;

		public static bool Contains(this Rect rect, Point[] points)
			=> points.Any(x => rect.Contains(x));

		public static Rect ApplyPadding(this Rect rect, Thickness thickness)
		{
			if (thickness == Thickness.Zero)
				return rect;
			rect.X += thickness.Left;
			rect.Y += thickness.Top;
			rect.Width -= thickness.HorizontalThickness;
			rect.Height -= thickness.VerticalThickness;

			return rect;
		}
		public static RectF ApplyPadding(this RectF rect, Thickness thickness)
		{
			if (thickness == Thickness.Zero)
				return rect;
			rect.X += (float)thickness.Left;
			rect.Y += (float)thickness.Top;
			rect.Width -= (float)thickness.HorizontalThickness;
			rect.Height -= (float)thickness.VerticalThickness;

			return rect;
		}

	}
}

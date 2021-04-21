using System;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public static class RectangleExtensions
	{
		public static bool BoundsContains(this Rectangle rect, Point point) =>
			point.X >= 0 && point.X <= rect.Width &&
			point.Y >= 0 && point.Y <= rect.Height;

		public static bool Contains(this Rectangle rect, Point[] points)
			=> points.Any(x => rect.Contains(x));

		public static Rectangle ApplyPadding(this Rectangle rect, Thickness thickness)
		{
			if (thickness == Thickness.Zero)
				return rect;
			rect.X += thickness.Left;
			rect.Y += thickness.Top;
			rect.Width -= thickness.HorizontalThickness;
			rect.Height -= thickness.VerticalThickness;

			return rect;
		}
		public static RectangleF ApplyPadding(this RectangleF rect, Thickness thickness)
		{
			if (thickness == Thickness.Zero)
				return rect;
			rect.X += (float)thickness.Left;
			rect.Y += (float)thickness.Top;
			rect.Width -= (float)thickness.HorizontalThickness;
			rect.Height -= (float)thickness.VerticalThickness;

			return rect;
		}

		public static bool BoundsContains(this RectangleF rect, PointF point) =>
			point.X >= 0 && point.X <= rect.Width &&
			point.Y >= 0 && point.Y <= rect.Height;

		public static bool Contains(this RectangleF rect, PointF[] points)
			=> points.Any(x => rect.Contains(x));

	}
}

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

using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public static class RectExtensions
	{
		public static PointF Center(this RectangleF rectangle) => new PointF(rectangle.X + (rectangle.Width / 2), rectangle.Y + (rectangle.Height / 2));
		public static void Center(this ref RectangleF rectangle, PointF center)
		{
			var halfWidth = rectangle.Width / 2;
			var halfHeight = rectangle.Height / 2;
			rectangle.X = center.X - halfWidth;
			rectangle.Y = center.Y - halfHeight;
		}
	}
}

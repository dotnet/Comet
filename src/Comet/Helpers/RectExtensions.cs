using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Comet
{
	public static class RectExtensions
	{
		public static Xamarin.Forms.Point Center(this Xamarin.Forms.Rectangle rectangle) => new Xamarin.Forms.Point(rectangle.X + (rectangle.Width / 2), rectangle.Y + (rectangle.Height / 2));
		public static void Center(this ref Xamarin.Forms.Rectangle rectangle, Xamarin.Forms.Point center)
		{
			var halfWidth = rectangle.Width / 2;
			var halfHeight = rectangle.Height / 2;
			rectangle.X = center.X - halfWidth;
			rectangle.Y = center.Y - halfHeight;
		}
	}
}

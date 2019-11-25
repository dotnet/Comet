using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Comet
{
	public static class RectExtensions
	{
		public static PointF Center(this RectangleF rectangle) => new PointF(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
	}
}

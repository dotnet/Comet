using System;
using System.Collections.Generic;
using System.Graphics;
namespace Comet
{
	public static class GraphicsExtensions
	{
		public static void AppendEllipse(this PathF path, float x, float y, float width, float height) => path.AppendOval(x, y, width, height);
		public static void AppendEllipse(this PathF path, RectangleF r) => path.AppendOval(r.X, r.Y, r.Width, r.Height);
		public static void AppendRectangle(this PathF path, RectangleF rect) => path.AppendRectangle(rect.X,rect.Y,rect.Width,rect.Height);
		public static void AppendRoundedRectangle(this PathF path, RectangleF rect, float radius) => path.AppendRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius);

	}
}

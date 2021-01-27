using System;
using System.Collections.Generic;
using System.Graphics;
namespace Comet
{
	public static class GraphicsExtensions
	{
		public static void AppendRectangle(this PathF path, Rectangle rect) => path.AppendRectangle(rect.X,rect.Y,rect.Width,rect.Height);
		public static void AppendRoundedRectangle(this PathF path, Rectangle rect, float radius) => path.AppendRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius);

	}
}

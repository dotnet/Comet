using System;
using System.Collections.Generic;
using System.Graphics;
namespace Comet
{
	public static class GraphicsExtensions
	{
		public static void AppendRectangle(this PathF path, Rectangle rect) => path.AppendRectangle((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
		public static void AppendRoundedRectangle(this PathF path, Rectangle rect, float radius) => path.AppendRoundedRectangle((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, radius);

	}
}

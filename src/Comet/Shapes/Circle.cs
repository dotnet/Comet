using System;
using System.Graphics;
using Comet.Graphics;

namespace Comet
{
	public class Circle : Shape
	{
		public override PathF PathForBounds(RectangleF rect)
		{
			var size = Math.Min(rect.Width, rect.Height);
			var x = rect.X + (rect.Width - size) / 2;
			var y = rect.Y + (rect.Height - size) / 2;
			var path = new PathF();
			path.AppendEllipse(x, y, size, size);
			return path;
		}
	}
}

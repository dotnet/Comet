using System;

using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class Circle : Shape
	{
		public override PathF PathForBounds(Rectangle rect)
		{
			var size = Math.Min(rect.Width, rect.Height);
			var x = rect.X + (rect.Width - size) / 2;
			var y = rect.Y + (rect.Height - size) / 2;
			var path = new PathF();
			path.AppendEllipse((float)x, (float)y, (float)size, (float)size);
			return path;
		}
	}
}

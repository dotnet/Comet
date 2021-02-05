using Comet.Graphics;
using System.Graphics;

namespace Comet
{
	public class Ellipse : Shape
	{
		public override PathF PathForBounds(Rectangle rect)
		{
			var path = new PathF();
			path.AppendEllipse(new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height));
			return path;
		}
	}
}

using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class Ellipse : Shape
	{
		public override PathF PathForBounds(Rect rect)
		{
			var path = new PathF();
			path.AppendEllipse(new RectF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height));
			return path;
		}
	}
}

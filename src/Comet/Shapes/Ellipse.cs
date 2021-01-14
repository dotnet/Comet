using Comet.Graphics;
using System.Graphics;

namespace Comet
{
	public class Ellipse : Shape
	{
		public override PathF PathForBounds(RectangleF rect)
		{
			var path = new PathF();
			path.AppendEllipse(rect);
			return path;
		}
	}
}

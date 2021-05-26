using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet.Shapes
{
	public class Rect : Shape
	{
		public override PathF PathForBounds(Rectangle rect, float density = 1)
		{
			var path = new PathF();
			path.AppendRectangle(rect);
			return path;
		}
	}
}

using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet.Shapes
{
	public class Rectangle : Shape
	{
		public override PathF PathForBounds(Microsoft.Maui.Graphics.Rectangle rect)
		{
			var path = new PathF();
			path.AppendRectangle(rect);
			return path;
		}
	}
}

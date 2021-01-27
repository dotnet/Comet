using Comet.Graphics;
using System.Graphics;

namespace Comet.Shapes
{
	public class Rectangle : Shape
	{
		public override PathF PathForBounds(System.Graphics.Rectangle rect)
		{
			var path = new PathF();
			path.AppendRectangle(rect);
			return path;
		}
	}
}

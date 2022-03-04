using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class Rectangle : Shape
	{
		public override PathF PathForBounds(Microsoft.Maui.Graphics.Rect rect)
		{
			var path = new PathF();
			path.AppendRectangle(rect);
			return path;
		}
	}
}

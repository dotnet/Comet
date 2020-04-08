using System.Maui.Graphics;
using System.Drawing;

namespace System.Maui
{
	public class Rectangle : Shape
	{
		public override PathF PathForBounds(RectangleF rect)
		{
			var path = new PathF();
			path.AppendRectangle(rect);
			return path;
		}
	}
}

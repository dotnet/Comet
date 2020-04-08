using System.Maui.Graphics;
using System.Drawing;

namespace System.Maui
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

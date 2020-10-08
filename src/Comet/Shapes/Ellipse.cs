using Comet.Graphics;

namespace Comet
{
	public class Ellipse : Shape
	{
		public override PathF PathForBounds(Xamarin.Forms.Rectangle rect)
		{
			var path = new PathF();
			path.AppendEllipse(rect);
			return path;
		}
	}
}

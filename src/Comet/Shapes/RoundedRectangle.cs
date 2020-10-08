using Comet.Graphics;
using System.Drawing;

namespace Comet
{
	public class RoundedRectangle : Shape
	{
		private readonly float _cornerRadius;

		public RoundedRectangle(float cornerRadius)
		{
			_cornerRadius = cornerRadius;
		}

		public float CornerRadius => _cornerRadius;

		public override PathF PathForBounds(Xamarin.Forms.Rectangle rect)
		{
			var path = new PathF();
			path.AppendRoundedRectangle(rect, _cornerRadius);
			return path;
		}
	}
}

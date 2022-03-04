using Comet.Graphics;
using Microsoft.Maui.Graphics;

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

		public override PathF PathForBounds(Rect rect)
		{
			var path = new PathF();
			path.AppendRoundedRectangle(rect, _cornerRadius);
			return path;
		}
	}
}

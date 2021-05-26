using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class RoundedRect : Shape
	{
		private readonly float _cornerRadius;

		public RoundedRect(float cornerRadius)
		{
			_cornerRadius = cornerRadius;
		}

		public float CornerRadius => _cornerRadius;

		public override PathF PathForBounds(Rectangle rect, float density = 1)
		{
			var path = new PathF();
			path.AppendRoundedRectangle(rect, _cornerRadius);
			return path;
		}
	}
}

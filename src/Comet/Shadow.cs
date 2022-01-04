

using Microsoft.Maui.Graphics;

namespace Comet.Graphics
{
	public class Shadow : IShadow
	{
		public Paint Paint { get; private set; } = new SolidPaint(Colors.Black);
		public float Opacity { get; private set; } = .5f;
		public float Radius { get; private set; } = 10;
		public Point Offset { get; private set; } = Point.Zero;

		float IShadow.Radius => Radius;

		float IShadow.Opacity => Opacity;

		Paint IShadow.Paint => Paint;

		Point IShadow.Offset => Offset;

		public Shadow()
		{

		}

		protected Shadow(Shadow prototype)
		{
			Paint = prototype.Paint;
			Opacity = prototype.Opacity;
			Radius = prototype.Radius;
			Offset = prototype.Offset;
		}

		public Shadow WithColor(Color color)
		{
			var shadow = new Shadow(this);
			shadow.Paint = new SolidPaint (color);
			return shadow;
		}
		public Shadow WithPaint(Paint paint)
		{
			var shadow = new Shadow(this);
			shadow.Paint = paint;
			return shadow;
		}

		public Shadow WithOpacity(float opacity)
		{
			var shadow = new Shadow(this);
			shadow.Opacity = opacity;
			return shadow;
		}

		public Shadow WithRadius(float radius)
		{
			var shadow = new Shadow(this);
			shadow.Radius = radius;
			return shadow;
		}

		public Shadow WithOffset(Point offset)
		{
			var shadow = new Shadow(this);
			shadow.Offset = offset;
			return shadow;
		}
	}
}

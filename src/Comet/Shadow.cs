using System.Graphics;

namespace Comet.Graphics
{
	public class Shadow
	{
		public Color Color { get; private set; } = Colors.Black;
		public float Opacity { get; private set; } = .5f;
		public float Radius { get; private set; } = 10;
		public Size Offset { get; private set; } = Size.Zero;

		public Shadow()
		{

		}

		protected Shadow(Shadow prototype)
		{
			Color = prototype.Color;
			Opacity = prototype.Opacity;
			Radius = prototype.Radius;
			Offset = prototype.Offset;
		}

		public Shadow WithColor(Color color)
		{
			var shadow = new Shadow(this);
			shadow.Color = color;
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

		public Shadow WithOffset(Size offset)
		{
			var shadow = new Shadow(this);
			shadow.Offset = offset;
			return shadow;
		}
	}
}

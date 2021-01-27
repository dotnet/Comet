using System.Graphics;

namespace Comet.Graphics
{
	public class RadialGradient : Gradient
	{
		public RadialGradient(Color[] colors, Point center, float startRadius, float endRadius) : base(colors)
		{
			Center = center;
			StartRadius = startRadius;
			EndRadius = endRadius;
		}

		public RadialGradient(Stop[] stops, Point center, float startRadius, float endRadius) : base(stops)
		{
			Center = center;
			StartRadius = startRadius;
			EndRadius = endRadius;
		}

		public Point Center { get; }

		public float StartRadius { get; }

		public float EndRadius { get; }
	}
}

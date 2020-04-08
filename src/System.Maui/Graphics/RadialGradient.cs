using System.Drawing;

namespace System.Maui.Graphics
{
	public class RadialGradient : Gradient
	{
		public RadialGradient(Color[] colors, PointF center, float startRadius, float endRadius) : base(colors)
		{
			Center = center;
			StartRadius = startRadius;
			EndRadius = endRadius;
		}

		public RadialGradient(Stop[] stops, PointF center, float startRadius, float endRadius) : base(stops)
		{
			Center = center;
			StartRadius = startRadius;
			EndRadius = endRadius;
		}

		public PointF Center { get; }

		public float StartRadius { get; }

		public float EndRadius { get; }
	}
}

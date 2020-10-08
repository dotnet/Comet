using System.Drawing;

namespace Comet.Graphics
{
	public class RadialGradient : Gradient
	{
		public RadialGradient(Color[] colors, Xamarin.Forms.Point center, float startRadius, float endRadius) : base(colors)
		{
			Center = center;
			StartRadius = startRadius;
			EndRadius = endRadius;
		}

		public RadialGradient(Stop[] stops, Xamarin.Forms.Point center, float startRadius, float endRadius) : base(stops)
		{
			Center = center;
			StartRadius = startRadius;
			EndRadius = endRadius;
		}

		public Xamarin.Forms.Point Center { get; }

		public float StartRadius { get; }

		public float EndRadius { get; }
	}
}

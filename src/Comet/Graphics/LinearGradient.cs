using System.Drawing;
using System.Net;

namespace Comet.Graphics
{
	public class LinearGradient : Gradient
	{
		public LinearGradient(Color[] colors, Xamarin.Forms.Point startPoint, Xamarin.Forms.Point endPoint) : base(colors)
		{
			StartPoint = startPoint;
			EndPoint = endPoint;
		}

		public LinearGradient(Stop[] stops, Xamarin.Forms.Point startPoint, Xamarin.Forms.Point endPoint) : base(stops)
		{
			StartPoint = startPoint;
			EndPoint = endPoint;
		}

		public Xamarin.Forms.Point StartPoint { get; }
		public Xamarin.Forms.Point EndPoint { get; }
	}
}

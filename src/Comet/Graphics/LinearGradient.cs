
using System.Net;
using Microsoft.Maui.Graphics;

namespace Comet.Graphics
{
	public class LinearGradient : Gradient
	{
		public LinearGradient(Color[] colors, Point startPoint, Point endPoint) : base(colors)
		{
			StartPoint = startPoint;
			EndPoint = endPoint;
		}

		public LinearGradient(Stop[] stops, Point startPoint, Point endPoint) : base(stops)
		{
			StartPoint = startPoint;
			EndPoint = endPoint;
		}

		public Point StartPoint { get; }
		public Point EndPoint { get; }
	}
}

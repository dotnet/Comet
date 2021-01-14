using System;
using System.Graphics;
using Comet.Graphics;

namespace Comet
{
	/// <summary>
	/// A capsule shape is equivalent to a rounded rectangle where the corner radius is chosen
	/// as half the length of the rectangle’s smallest edge.
	/// </summary>
	public class Capsule : Shape
	{
		public override PathF PathForBounds(RectangleF rect)
		{
			var path = new PathF();
			var cornerSize = Math.Min(rect.Width, rect.Height) / 2;
			path.AppendRoundedRectangle(rect, cornerSize);
			return path;
		}
	}
}

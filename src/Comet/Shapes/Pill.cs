using System;
using System.Drawing;
using Comet.Graphics;

namespace Comet
{
	public class Pill : Shape
	{
		public Pill(Orientation orientation)
		{
			Orientation = orientation;
		}

		public Orientation Orientation { get; }

		public override PathF PathForBounds(RectangleF rect)
		{
			var cornerRadius = (Orientation == Orientation.Horizontal ? rect.Height : rect.Width) / 2f;
			var path = new PathF();
			path.AppendRoundedRectangle(rect, cornerRadius);
			return path;
		}
	}
}

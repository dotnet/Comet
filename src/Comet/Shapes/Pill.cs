using System;

using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class Pill : Shape
	{
		public Pill(Orientation orientation)
		{
			Orientation = orientation;
		}

		public Orientation Orientation { get; }

		public override PathF PathForBounds(Rectangle rect, float density = 1)
		{
			var cornerRadius = (float)(Orientation == Orientation.Horizontal ? rect.Height : rect.Width) / 2f;
			var path = new PathF();
			path.AppendRoundedRectangle(rect, cornerRadius);
			return path;
		}
	}
}

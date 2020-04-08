using System;
using System.Drawing;
using System.Maui.Graphics;

namespace System.Maui
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

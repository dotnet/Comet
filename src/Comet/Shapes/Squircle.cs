using System;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;

namespace Comet;

public class Squircle : Shape
{
	private readonly float cornerRadius;

	public Squircle(float cornerRadius)
	{
		this.cornerRadius = cornerRadius;
	}

	public float CornerRadius => cornerRadius;
	const float SquircleFactor = 1.125f;
	const float ControlPointFactor = 46f / 64f;
	public override PathF PathForBounds(Microsoft.Maui.Graphics.Rectangle rect)
	{
		var x = (float)rect.X;
		var y = (float)rect.Y;
		var w = (float)rect.Width;
		var h = (float)rect.Height;
		var path = new PathF();
		var adjustedRadius = cornerRadius * SquircleFactor;
		adjustedRadius = Math.Min(adjustedRadius, Math.Min(w, h) / 2);
		var controlPointOffset = adjustedRadius * ControlPointFactor;

		var x1 = x + adjustedRadius;
		var x3 = x + w;
		var x2 = x3 - adjustedRadius;

		var y1 = y + adjustedRadius;
		var y3 = y + h;
		var y2 = y3 - adjustedRadius;

		path.MoveTo(x1, y);
		path.LineTo(x2, y);
		path.CurveTo(
			x2 + controlPointOffset, y,
			x3, y1 - controlPointOffset,
			x3, y1);
		path.LineTo(x3, y2);
		path.CurveTo(
			x3, y2 + controlPointOffset,
			x2 + controlPointOffset, y3,
			x2, y3);
		path.LineTo(x1, y3);
		path.CurveTo(
			x1 - controlPointOffset, y3,
			x, y2 + controlPointOffset,
			x, y2);
		path.LineTo(x, y1);
		path.CurveTo(
			x, y1 - controlPointOffset,
			x1 - controlPointOffset, y,
			x1, y);
		path.Close();
		return path;
	}
}

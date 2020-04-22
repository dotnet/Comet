using Comet.Graphics;
using SkiaSharp;
using System.Drawing;

namespace Comet.Skia
{
	public class SkiaShapeView : SkiaView
	{
		public Shape Shape { get; }

		public SkiaShapeView(Shape shape)
		{
			Shape = shape;
		}

		public override void Draw(SKCanvas canvas, RectangleF rect)
		{
			var backgroundColor = this.GetBackgroundColor(Color.Transparent).ToSKColor();
			canvas.Clear(backgroundColor);
			var drawingStyle = Shape.GetDrawingStyle(this, DrawingStyle.StrokeFill);
			var strokeColor = Shape.GetStrokeColor(this, Color.Black);
			var strokeWidth = Shape.GetLineWidth(this, 1);
			var fill = Shape.GetFill(this);
			canvas.DrawShape(Shape, rect, drawingStyle, strokeWidth, strokeColor, fill);
		}
	}
}

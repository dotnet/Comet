using System;
using HotUI.Graphics;
using SkiaSharp;

namespace HotUI.Skia
{
    public class SkiaShapeView : AbstractControlDelegate
    {
        public Shape Shape;

        public SkiaShapeView(Shape shape)
        {
            Shape = shape;
        }

        public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {

            canvas.Clear(SKColors.White);

            var paint = new SKPaint()
            {
                Color = Shape.GetStrokeColor(this, Color.Black).ToSKColor(),
                StrokeWidth = Shape.GetLineWidth(this, 1),
                Style = SKPaintStyle.Stroke
            };

            var drawingStyle = Shape.GetDrawingStyle(this, DrawingStyle.StrokeFill);

            if (drawingStyle == DrawingStyle.Fill || drawingStyle == DrawingStyle.StrokeFill)
            {
                paint.Style = SKPaintStyle.StrokeAndFill;
            }

            var path = Shape.PathForBounds(dirtyRect).ToSKPath();

            canvas.DrawPath(path, paint);

        }
    }
}

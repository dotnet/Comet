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

        public override void Draw(SKCanvas canvas, RectangleF rect)
        {
            canvas.Clear(SKColors.White);

            if (Shape != null)
            {
                SKPaint strokePaint = null;
                SKPaint fillPaint = null;
                SKShader shader = null;
                object fill = null;
                float lineWidth = 0;
                
                var drawingStyle = Shape.GetDrawingStyle(VirtualDrawableControl, DrawingStyle.StrokeFill);
                
                switch (drawingStyle)
                {
                    case DrawingStyle.StrokeFill:
                        strokePaint = new SKPaint();
                        strokePaint.StrokeWidth = lineWidth = Shape.GetLineWidth(VirtualDrawableControl, 1);
                        strokePaint.Color = Shape.GetStrokeColor(VirtualDrawableControl, Color.Black).ToSKColor();
                        strokePaint.Style = SKPaintStyle.Stroke;
                        fillPaint = new SKPaint();
                        fillPaint.Style = SKPaintStyle.Fill;
                        fill = Shape.GetFill(VirtualDrawableControl);
                        break;
                    case DrawingStyle.Stroke:
                        strokePaint = new SKPaint();
                        strokePaint.StrokeWidth = lineWidth = Shape.GetLineWidth(VirtualDrawableControl, 1);
                        strokePaint.Color = Shape.GetStrokeColor(VirtualDrawableControl, Color.Black).ToSKColor();
                        strokePaint.Style = SKPaintStyle.Stroke;
                        fillPaint = new SKPaint();
                        fillPaint.Style = SKPaintStyle.Fill;
                        break;
                    case DrawingStyle.Fill:
                        fillPaint = new SKPaint();
                        fillPaint.Style = SKPaintStyle.Fill;
                        fill = Shape.GetFill(VirtualDrawableControl);
                        break;
                }
                
                var shapeBounds = new RectangleF(
                    rect.X + (lineWidth / 2),
                    rect.Y + (lineWidth / 2),
                    rect.Width - lineWidth,
                    rect.Height - lineWidth);
                
                var path = Shape.PathForBounds(shapeBounds).ToSKPath();
                
                if (fill != null && fillPaint != null)
                {
                    if (fill is Color color)
                    {
                        fillPaint.Color = color.ToSKColor();
                        canvas.DrawPath(path, fillPaint);
                    }
                    else if (fill is Gradient gradient)
                    {
                        canvas.Save();
                        
                        var colors = new SKColor[gradient.Stops.Length];
                        var stops = new float[colors.Length];

                        var sortedStops = gradient.GetSortedStops();

                        for (var i = 0; i < sortedStops.Length; i++)
                        {
                            colors[i] = sortedStops[i].Color.ToSKColor();
                            stops[i] = sortedStops[i].Offset;
                        }
                        
                        if (gradient is LinearGradient linearGradient)
                        {
                            var x1 = rect.X + rect.Width * linearGradient.StartPoint.X;
                            var y1 = rect.Y + rect.Height * linearGradient.StartPoint.Y;

                            var x2 = rect.X + rect.Width * linearGradient.EndPoint.X;
                            var y2 = rect.Y + rect.Height * linearGradient.EndPoint.Y;

                            shader = SKShader.CreateLinearGradient(
                                new SKPoint(x1, y1),
                                new SKPoint(x2, y2),
                                colors,
                                stops,
                                SKShaderTileMode.Clamp);
                        }
                        else if (gradient is RadialGradient radialGradient)
                        {
                            var x1 = rect.X + rect.Width * radialGradient.Center.X;
                            var y1 = rect.Y + rect.Height * radialGradient.Center.Y;
                            var r = radialGradient.EndRadius;
                            
                            shader = SKShader.CreateRadialGradient(
                                new SKPoint(x1, y1),
                                r,
                                colors,
                                stops,
                                SKShaderTileMode.Clamp);
                        }

                        if (shader != null)
                        {
                            fillPaint.Shader = shader;
                            canvas.DrawPath(path, fillPaint);
                        }

                        shader.Dispose();
                        canvas.Restore();
                    }
                }

                if (strokePaint != null)
                    canvas.DrawPath(path, strokePaint);
                
                strokePaint?.Dispose();
                fillPaint?.Dispose();
            }
        }
    }
}

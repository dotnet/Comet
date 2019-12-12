using System;
using System.Drawing;
using Comet.Graphics;
using SkiaSharp;

namespace Comet.Skia
{
	public static class SkiaDrawingExtensions
	{
		public static void DrawShape(
			this SKCanvas canvas, 
			Shape shape, 
			RectangleF rect, 
			DrawingStyle drawingStyle = DrawingStyle.Fill, 
			float strokeWidth = 1, 
			Color strokeColor = null, 
			object fill = null)
		{
			if (shape == null)
				return;
			SKPaint strokePaint = null;
			SKPaint fillPaint = null;
			SKShader shader = null;
			float lineWidth = 0;

			switch (drawingStyle)
			{
				case DrawingStyle.StrokeFill:
					strokePaint = new SKPaint
					{
						IsAntialias = true,
						StrokeWidth = lineWidth = strokeWidth,
						Color = (strokeColor ?? Color.Black).ToSKColor(),
						Style = SKPaintStyle.Stroke
					};
					fillPaint = new SKPaint
					{
						IsAntialias = true,
						Style = SKPaintStyle.Fill
					};
					break;
				case DrawingStyle.Stroke:
					strokePaint = new SKPaint
					{
						IsAntialias = true,
						StrokeWidth = lineWidth = strokeWidth,
						Color = (strokeColor ?? Color.Black).ToSKColor(),
						Style = SKPaintStyle.Stroke
					};
					fillPaint = new SKPaint
					{
						IsAntialias = true,
						Style = SKPaintStyle.Fill
					};
					break;
				case DrawingStyle.Fill:
					fillPaint = new SKPaint
					{
						IsAntialias = true,
						Style = SKPaintStyle.Fill
					};
					break;
			}

			var shapeBounds = new RectangleF(
				rect.X + (lineWidth / 2),
				rect.Y + (lineWidth / 2),
				rect.Width - lineWidth,
				rect.Height - lineWidth);

			var path = shape.PathForBounds(shapeBounds).ToSKPath();

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

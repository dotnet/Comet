using System;
using System.Collections.Generic;
using Comet.Graphics;
using Comet.Internal;
using Microsoft.Maui.Graphics;

namespace Comet.Graphics
{
	public static class GraphicsExtensions
	{
		public static Paint ConvertToPaint(this Object obj)
		{
			if (obj == null)
				return null;
			var p = obj?.GetValueOfType<Paint>();
			if (p != null)
				return p;
			var c = obj?.GetValueOfType<Color>();
			if (c != null)
				return new SolidPaint { Color = c };
			var s = obj?.GetValueOfType<string>();
			if (!string.IsNullOrWhiteSpace(s))
			{
				return new SolidPaint
				{
					Color = Color.FromArgb(s),
				};
			}
			return null;
		}
	}
}
namespace Comet
{
	public static class GraphicsExtensions
	{
		public static void AppendRectangle(this PathF path, Rectangle rect) => path.AppendRectangle((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
		public static void AppendRoundedRectangle(this PathF path, Rectangle rect, float radius) => path.AppendRoundedRectangle((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, radius);


		public static void DrawShape(
			this ICanvas canvas,
			Shape shape,
			RectangleF rect,
			DrawingStyle drawingStyle = DrawingStyle.Fill,
			float strokeWidth = 1,
			Color strokeColor = null,
			object fill = null)
		{
			if (shape == null)
				return;
			Paint strokePaint = null;
			Paint fillPaint = null;
			//Graphics.sh shader = null;
			float lineWidth = 0;

			switch (drawingStyle)
			{
				case DrawingStyle.StrokeFill:
					strokePaint = new SolidPaint
					{
						BackgroundColor = strokeColor,
					};

					fillPaint = new SolidPaint
					{
						//BackgroundColor = strokeColor,
					};
					break;
				case DrawingStyle.Stroke:
					strokePaint = new SolidPaint
					{

						BackgroundColor = strokeColor,
					};
					fillPaint = new SolidPaint();
					break;
				case DrawingStyle.Fill:
					fillPaint = new SolidPaint();
					break;
			}

			var shapeBounds = new RectangleF(
				rect.X + (lineWidth / 2),
				rect.Y + (lineWidth / 2),
				rect.Width - lineWidth,
				rect.Height - lineWidth);

			var path = shape.PathForBounds(shapeBounds);

			if (fill != null && fillPaint != null)
			{
				if (fill is Color color)
				{
					canvas.FillColor = color;
					canvas.FillPath(path);
				}
				else if (fill is Gradient gradient)
				{
					canvas.SaveState();

					var colors = new GradientStop[gradient.Stops.Length];

					var sortedStops = gradient.GetSortedStops();

					for (var i = 0; i < sortedStops.Length; i++)
					{
						colors[i] = new GradientStop(sortedStops[i].Offset,sortedStops[i].Color);
					}

					if (gradient is LinearGradient linearGradient)
					{
						var x1 = (float)(rect.X + rect.Width * linearGradient.StartPoint.X);
						var y1 = (float)(rect.Y + rect.Height * linearGradient.StartPoint.Y);

						var x2 = (float)(rect.X + rect.Width * linearGradient.EndPoint.X);
						var y2 = (float)(rect.Y + rect.Height * linearGradient.EndPoint.Y);
						fillPaint = new LinearGradientPaint
						{
							GradientStops = colors,
						};
						canvas.SetFillPaint(fillPaint,new RectangleF( x1, y1, x2, y2));
						canvas.FillPath(path);
					}
					else if (gradient is RadialGradient radialGradient)
					{
						var x1 = (float)(rect.X + rect.Width * radialGradient.Center.X);
						var y1 = (float)(rect.Y + rect.Height * radialGradient.Center.Y);
						var x2 = x1;
						var y2 = (float)(y1  + radialGradient.EndRadius);
						fillPaint = new RadialGradientPaint
						{
							GradientStops = colors,
						};

						canvas.SetFillPaint(fillPaint, new RectangleF(x1, y1, x2, y2));
						canvas.FillPath(path);
					}

					//if (shader != null)
					//{
					//	fillPaint.Shader = shader;
					//	canvas.DrawPath(path, fillPaint);
					//}

					//shader.Dispose();
					canvas.RestoreState();
				}
			}

			if (strokePaint != null)
			{
				canvas.StrokeSize = strokeWidth;
				canvas.StrokeColor = strokeColor;
				canvas.DrawPath(path);
			}

		}
	}
}

using System;
using System.Collections.Generic;
using Comet.Graphics;
using Microsoft.Maui.Graphics;

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
					strokePaint = new Paint
					{
						PaintType = PaintType.Solid,
						BackgroundColor = strokeColor,
					};

					fillPaint = new Paint
					{
						PaintType = PaintType.Solid,
						//BackgroundColor = strokeColor,
					};
					break;
				case DrawingStyle.Stroke:
					strokePaint = new Paint
					{

						PaintType = PaintType.Solid,
						BackgroundColor = strokeColor,
					};
					fillPaint = new Paint
					{
						PaintType = PaintType.Solid,
					};
					break;
				case DrawingStyle.Fill:
					fillPaint = new();
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
					fillPaint.BackgroundColor = color;
					canvas.SetFillPaint(fillPaint, shapeBounds);
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
						var x1 = rect.X + rect.Width * linearGradient.StartPoint.X;
						var y1 = rect.Y + rect.Height * linearGradient.StartPoint.Y;

						var x2 = rect.X + rect.Width * linearGradient.EndPoint.X;
						var y2 = rect.Y + rect.Height * linearGradient.EndPoint.Y;
						fillPaint.PaintType = PaintType.LinearGradient;
						fillPaint.Stops = colors;
						canvas.SetFillPaint(fillPaint, shapeBounds);
						canvas.FillPath(path);
					}
					else if (gradient is RadialGradient radialGradient)
					{
						var x1 = rect.X + rect.Width * radialGradient.Center.X;
						var y1 = rect.Y + rect.Height * radialGradient.Center.Y;
						var r = radialGradient.EndRadius;


						fillPaint.PaintType = PaintType.RadialGradient;
						fillPaint.Stops = colors;

						canvas.SetFillPaint(fillPaint, shapeBounds);
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

using System;
using CoreGraphics;
using Comet.Graphics;
using UIKit;
using System.Drawing;

namespace Comet.iOS
{
	public class CUIShapeView : UIView
	{
		private static readonly CGColorSpace ColorSpace = CGColorSpace.CreateDeviceRGB();

		private Shape _shape;

		public CUIShapeView()
		{
			BackgroundColor = UIColor.Clear;
		}

		public Shape Shape
		{
			get => _shape;
			set
			{
				_shape = value;
				SetNeedsDisplay();
			}
		}

		public View View { get; set; }

		public override void Draw(CGRect rect)
		{
			var context = UIGraphics.GetCurrentContext();
			if (Shape != null)
			{
				var drawingStyle = Shape.GetDrawingStyle(View, DrawingStyle.StrokeFill);

				var lineWidth = Shape.GetLineWidth(View, 1);
				var strokeColor = Color.Black;
				object fill = null;

				if (drawingStyle == DrawingStyle.Fill || drawingStyle == DrawingStyle.StrokeFill)
				{
					fill = Shape.GetFill(View);
				}

				var shapeBounds = new RectangleF(
					(float)rect.X + (lineWidth / 2),
					(float)rect.Y + (lineWidth / 2),
					(float)rect.Width - lineWidth,
					(float)rect.Height - lineWidth);

				var path = Shape.PathForBounds(shapeBounds).ToCGPath();

				if (fill != null)
				{
					if (fill is Color color)
					{
						context.SetFillColor(color.ToCGColor());
						context.AddPath(path);
						context.FillPath();
					}
					else if (fill is Gradient gradient)
					{
						context.SaveState();
						context.AddPath(path);
						context.Clip();

						var gradientColors = new nfloat[gradient.Stops.Length * 4];
						var offsets = new nfloat[gradient.Stops.Length];

						int g = 0;
						for (int i = 0; i < gradient.Stops.Length; i++)
						{
							var stopColor = gradient.Stops[i].Color;
							offsets[i] = gradient.Stops[i].Offset;

							if (stopColor == null) stopColor = Color.White;

							gradientColors[g++] = stopColor.R;
							gradientColors[g++] = stopColor.G;
							gradientColors[g++] = stopColor.B;
							gradientColors[g++] = stopColor.A;
						}

						var cgGradient = new CGGradient(CGColorSpace.CreateDeviceRGB(), gradientColors, offsets);

						if (gradient is LinearGradient linearGradient)
						{
							var gradientStart = new CGPoint(
								(float)rect.X + rect.Width * linearGradient.StartPoint.X,
								(float)rect.Y + rect.Height * linearGradient.StartPoint.Y);

							var gradientEnd = new CGPoint(
								(float)rect.X + rect.Width * linearGradient.EndPoint.X,
								(float)rect.Y + rect.Height * linearGradient.EndPoint.Y);

							context.DrawLinearGradient(
								cgGradient,
								gradientStart,
								gradientEnd,
								CGGradientDrawingOptions.DrawsAfterEndLocation | CGGradientDrawingOptions.DrawsBeforeStartLocation);
						}
						else if (gradient is RadialGradient radialGradient)
						{
							var _radialFocalPoint = new CGPoint(
								(float)rect.X + rect.Width * radialGradient.Center.X,
								(float)rect.Y + rect.Height * radialGradient.Center.Y);

							context.DrawRadialGradient(
								cgGradient,
								_radialFocalPoint,
								radialGradient.StartRadius,
								_radialFocalPoint,
								radialGradient.EndRadius,
								CGGradientDrawingOptions.DrawsBeforeStartLocation | CGGradientDrawingOptions.DrawsAfterEndLocation);
						}

						cgGradient.Dispose();
						context.RestoreState();
					}
				}

				if (drawingStyle == DrawingStyle.Stroke || drawingStyle == DrawingStyle.StrokeFill)
				{
					strokeColor = Shape.GetStrokeColor(View, Color.Black);

					context.SetLineWidth(lineWidth);
					context.SetStrokeColor(strokeColor.ToCGColor());

					context.AddPath(path);
					context.StrokePath();
				}
			}
		}
	}
}

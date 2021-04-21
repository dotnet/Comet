using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
namespace Comet.GraphicsControls
{
	public class MaterialButtonHandler : ViewDrawable<IButton>, IButtonDrawable
	{
		static float hPadding = 40;
		static float minHPadding = 10;
		static float vPadding = 10;

	
		static Font defaultFont = Font.Default.WithSize(14).WithAttributes(FontAttributes.Bold);

		public override Size GetDesiredSize(IView view, double widthConstraint, double heightConstraint)
		{
			var font = VirtualView.Font.IsDefault ? defaultFont : VirtualView.Font;
			var size = GraphicsPlatform.CurrentService.GetStringSize(VirtualView.Text, font.FontFamily, (float)font.FontSize);
			size.Width += hPadding;
			size.Height += vPadding;
			return new Size(Math.Min(size.Width, widthConstraint), Math.Min(size.Height, heightConstraint));
		}
		public override void DrawBackground(ICanvas canvas, RectangleF dirtyRect, IView view)
		{
			var defaultColor = VirtualView.BackgroundColor ?? Colors.Transparent;

			Color calculateDefaultBackgroundColor()
			{
				var c = defaultColor;
				c = CurrentState switch
				{
					ControlState.Disabled => c.Lerp(Colors.Grey, .5),
					ControlState.Hovered => c.Lerp(Colors.Grey, .1),
					ControlState.Pressed => c.Lerp(Colors.Grey, .5),
					_ => c
				};
				return c;

			};
			
			var backgroundColor = calculateDefaultBackgroundColor();

			canvas.FillColor = backgroundColor;
			canvas.FillRectangle(dirtyRect);
			//var radius = (this).GetEnvironment<float>(accentRadius);
			//if (radius <= 0 || radius >= 1)
			//{
			//	base.DrawBackground(canvas, defaultColor, dirtyRect);
			//	base.DrawBackground(canvas, backgroundColor, dirtyRect);
			//	return;
			//}
			//base.DrawBackground(canvas, defaultColor, dirtyRect);
			//var paint = new SKPaint();
			//paint.Color = backgroundColor.ToSKColor();
			//var circleRadius = 5f.Lerp(Math.Max(dirtyRect.Width, dirtyRect.Height) * 2f, radius);
			//canvas.DrawCircle(animationPoint, circleRadius, paint);
		}
		//public override string[] LayerDrawingOrder() => throw new NotImplementedException();
	}
}

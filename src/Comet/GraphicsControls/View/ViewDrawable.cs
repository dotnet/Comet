using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.GraphicsControls
{

	public class ViewDrawable<TVirtualView> : ViewDrawable, IViewDrawable<TVirtualView>
		where TVirtualView : IView
	{

		public TVirtualView VirtualView { get => (TVirtualView)View; set => View = value; }
	}

	public class ViewDrawable : IViewDrawable
	{
		public IView View { get; set; }
		public ControlState CurrentState { get; set; }

		public virtual void DrawBackground(ICanvas canvas, RectangleF dirtyRect, IView view)
		{
			if (view.BackgroundColor == null)
				return;
			canvas.FillColor = view.BackgroundColor;
			canvas.DrawRectangle(dirtyRect);
		}

		public virtual void DrawClip(ICanvas canvas, RectangleF dirtyRect, IView view)
		{
			//TODO: Bring back when we have a clip shape!
		}

		public virtual void DrawText(ICanvas canvas, RectangleF dirtyRect, IText text)
		{
			if (text == null)
				return;
			canvas.FontColor = text.TextColor ?? Colors.Black;
			canvas.FontName = text.Font.FontFamily;
			canvas.FontSize = (float)text.Font.FontSize;

			//TODO: Account for Left -> Right
			var horizontal =
				((text as ITextAlignment)?.HorizontalTextAlignment ?? TextAlignment.Center) switch
				{
					TextAlignment.Start => Microsoft.Maui.Graphics.HorizontalAlignment.Left,
					TextAlignment.Center => Microsoft.Maui.Graphics.HorizontalAlignment.Center,
					TextAlignment.End => Microsoft.Maui.Graphics.HorizontalAlignment.Right,
					_ => Microsoft.Maui.Graphics.HorizontalAlignment.Center,
				};

			canvas.DrawString((string)text.Text, dirtyRect, horizontalAlignment: horizontal, verticalAlignment: Microsoft.Maui.Graphics.VerticalAlignment.Center);

		}
		public virtual void DrawOverlay(ICanvas canvas, RectangleF dirtyRect, IView view)
		{

		}

		public virtual void DrawBorder(ICanvas canvas, RectangleF dirtyRect, IView view)
		{

		}


		public virtual Size GetDesiredSize(IView view, double widthConstraint, double heightConstraint) => new Size(100, 44);

	}

	
}

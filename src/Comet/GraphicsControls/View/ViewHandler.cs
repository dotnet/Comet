using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.GraphicsControls
{
	public class ViewHandler
	{
		public static string[] DefaultLayerDrawingOrder = new[]
		{
			"Clip",
			"Background",
			"Border",
			"Text",
			"Overlay",
		};

		public static DrawMapper<IViewHandler, IView> DrawMapper = new DrawMapper<IViewHandler, IView>
		{
			["Clip"] = DrawClip,
			["Background"] = DrawBackground,
			["Border"] = DrawBorder,
			["Text"] = DrawText,
			["Overlay"] = DrawOverlay,
		};

		public new static readonly PropertyMapper<IView> Mapper = new PropertyMapper<IView>(Microsoft.Maui.Handlers.ViewHandler.ViewMapper)
		{

			[nameof(IView.AutomationId)] = MapInvalidate,
			[nameof(IView.BackgroundColor)] = MapInvalidate,
			[nameof(IView.IsEnabled)] = MapInvalidate,
			[nameof(IText.Text)] = MapInvalidate,
			[nameof(IText.Font)] = MapInvalidate,
		};

		public static void MapInvalidate(IViewHandler handler, IView view) => (handler as IGraphicsControl)?.Invalidate();

		private static void DrawClip(ICanvas canvas, RectangleF dirtyRect, IViewHandler handler, IView view)
		{
			//TODO: Bring back when we have a clip shape!
		}

		private static void DrawBackground(ICanvas canvas, RectangleF dirtyRect, IViewHandler handler, IView view)
		{
			if (view.BackgroundColor == null)
				return;
			canvas.FillColor = view.BackgroundColor;
			canvas.DrawRectangle(dirtyRect);
		}

		private static void DrawBorder(ICanvas canvas, RectangleF dirtyRect, IViewHandler handler, IView view)
		{
			//TODO: Bring back when we have a clip shape!

		}

		private static void DrawText(ICanvas canvas, RectangleF dirtyRect, IViewHandler handler, IView view)
		{
			var t = view as IText;
			if (t == null)
				return;
			canvas.FillColor = t.TextColor ?? Colors.Black;
			canvas.FontName = t.Font.FontFamily;
			canvas.FontSize = (float)t.Font.FontSize;

			//TODO: Account for Left -> Right
			var horizontal =
				((view as ITextAlignment)?.HorizontalTextAlignment ?? TextAlignment.Center) switch
				{
					TextAlignment.Start => Microsoft.Maui.Graphics.HorizontalAlignment.Left,
					TextAlignment.Center => Microsoft.Maui.Graphics.HorizontalAlignment.Center,
					TextAlignment.End => Microsoft.Maui.Graphics.HorizontalAlignment.Right,
					_ => Microsoft.Maui.Graphics.HorizontalAlignment.Center,
				};

			canvas.DrawString(t.Text, dirtyRect,horizontalAlignment: horizontal, verticalAlignment: Microsoft.Maui.Graphics.VerticalAlignment.Center);

		}

		private static void DrawOverlay(ICanvas canvas, RectangleF dirtyRect, IViewHandler handler, IView view)
		{

		}

	}
}

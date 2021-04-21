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

		public static DrawMapper<IViewDrawable, IView> DrawMapper = new()
		{
			["Clip"] = DrawClip,
			["Background"] = DrawBackground,
			["Border"] = DrawBorder,
			["Text"] = DrawText,
			["Overlay"] = DrawOverlay,
		};

		public static void DrawClip(ICanvas canvas, RectangleF dirtyRect, IViewDrawable drawable, IView view) => drawable.DrawClip(canvas, dirtyRect, view);
		public static void DrawBackground(ICanvas canvas, RectangleF dirtyRect, IViewDrawable drawable, IView view) => drawable.DrawBackground(canvas, dirtyRect, view);
		public static void DrawBorder(ICanvas canvas, RectangleF dirtyRect, IViewDrawable drawable, IView view) => drawable.DrawBorder(canvas, dirtyRect, view);
		public static void DrawText(ICanvas canvas, RectangleF dirtyRect, IViewDrawable drawable, IView view) {
			if(view is IText text)
				drawable.DrawText(canvas, dirtyRect, text);
		}
		public static void DrawOverlay(ICanvas canvas, RectangleF dirtyRect, IViewDrawable drawable, IView view) => drawable.DrawOverlay(canvas, dirtyRect, view);

		public new static readonly PropertyMapper<IView> Mapper = new PropertyMapper<IView>(Microsoft.Maui.Handlers.ViewHandler.ViewMapper)
		{
			Actions = {
				[nameof(IView.AutomationId)] = MapInvalidate,
				[nameof(IView.BackgroundColor)] = MapInvalidate,
				[nameof(IView.IsEnabled)] = MapInvalidate,
				[nameof(IText.Text)] = MapInvalidate,
				[nameof(IText.Font)] = MapInvalidate,
			}
		};

		public static void MapInvalidate(IViewHandler handler, IView view) => (handler as IGraphicsControl)?.Invalidate();
	}
}

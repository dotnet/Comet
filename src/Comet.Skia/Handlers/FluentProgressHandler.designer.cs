using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Comet.Internal;
using SkiaSharp;
using Topten.RichTextKit;
using Comet.Skia.Internal;
namespace Comet.Skia
{
	public partial class FluentProgressHandler
	{
		public FluentProgressHandler() : base(ProgressBarDrawMapper, Mapper) { }
		public static DrawMapper<ProgressBar> ProgressBarDrawMapper = new DrawMapper<ProgressBar>(SkiaControl.DrawMapper)
		{
			["Background"] = MapDrawBackground,
			["BackgroundRect"] = MapDrawBackgroundRect,
			["Progress"] = MapDrawProgress,
		};
		public static string[] DefaultProgressBarLayerDrawingOrder = new []
		{
			SkiaEnvironmentKeys.Clip,
			"Background",
			"BackgroundRect",
			"Progress",
			SkiaEnvironmentKeys.Overlay
		};
		protected override string[] LayerDrawingOrder() => DefaultProgressBarLayerDrawingOrder;
		static Dictionary<string, object> stateDefaultValues = new Dictionary<string, object>
		{
			["BackgroundColor"] = new Color(1f, 1f, 1f, 1f),
			["🎨 BackgroundRectRect"] = new RectangleF(0f, 0f,375f,2f),
			["🎨 BackgroundRectFill"] = new Color(0.8824f, 0.8824f, 0.8824f, 1f),
			["🎨 ProgressRect"] = new RectangleF(0f, 0f,120f,2f),
			["🎨 ProgressFill"] = new Color(0f, 0.47058824f, 0.83137256f, 1f),
		};
		static string[] stateKeys = new []
		{
			"BackgroundColor",
			"🎨 BackgroundRectRect",
			"🎨 BackgroundRectFill",
			"🎨 ProgressRect",
			"🎨 ProgressFill",
		};
		protected virtual void DrawBackground (SKCanvas canvas, RectangleF rectangle)
		{
			var bgRect = new Rectangle();
			canvas.DrawShape(bgRect, rectangle, fill: this.GetEnvironment<Color>("BackgroundColor"));
			
		}
		protected virtual void DrawBackgroundRect (SKCanvas canvas, RectangleF rectangle)
		{
			//Drawing Rounded Rect: 🎨 BackgroundRect
			{
				var bgRect = new Rectangle();
				canvas.DrawShape(bgRect, this.GetEnvironment<RectangleF>("🎨 BackgroundRectRect"), fill: this.GetEnvironment<Color>("🎨 BackgroundRectFill"));
			}
			
		}
		protected virtual void DrawProgress (SKCanvas canvas, RectangleF rectangle)
		{
			//Drawing Rounded Rect: 🎨 Progress
			{
				var bgRect = new Rectangle();
				canvas.DrawShape(bgRect, this.GetEnvironment<RectangleF>("🎨 ProgressRect"), fill: this.GetEnvironment<Color>("🎨 ProgressFill"));
			}
			
		}
		static void MapDrawBackground (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, ProgressBar view)
		{
			var typed = control as FluentProgressHandler;
			typed?.DrawBackground (canvas, dirtyRect);
		}
		static void MapDrawBackgroundRect (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, ProgressBar view)
		{
			var typed = control as FluentProgressHandler;
			typed?.DrawBackgroundRect (canvas, dirtyRect);
		}
		static void MapDrawProgress (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, ProgressBar view)
		{
			var typed = control as FluentProgressHandler;
			typed?.DrawProgress (canvas, dirtyRect);
		}
	}
}

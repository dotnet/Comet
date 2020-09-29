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
	public partial class FluentButtonHandler
	{
		public FluentButtonHandler() : base(ButtonDrawMapper, Mapper) { }
		public static DrawMapper<Button> ButtonDrawMapper = new DrawMapper<Button>(SkiaControl.DrawMapper)
		{
			["Background"] = MapDrawBackground,
			["Content"] = MapDrawContent,
		};
		public static string[] DefaultButtonLayerDrawingOrder = new []
		{
			SkiaEnvironmentKeys.Clip,
			"Background",
			"Content",
			SkiaEnvironmentKeys.Overlay
		};
		protected override string[] LayerDrawingOrder() => DefaultButtonLayerDrawingOrder;
		static Dictionary<string, object> stateDefaultValues = new Dictionary<string, object>
		{
			["BackgroundColor"] = new Color(0f, 0.47058824f, 0.83137256f, 1f),
			["Content✏️ Label"] = "Default",
			["Content✏️ Label.Font"] = new FontAttributes{Family = "SF Pro Text", Size = 15, Weight = (Weight)500 },
			["Content✏️ Label.Color"] = new Color(1f, 1f, 1f, 1f),
			["BackgroundColor.Pressed"] = new Color(0.16862746f, 0.53333336f, 0.84705883f, 1f),
			["Content✏️ Label.Pressed"] = "Pressed",
			["Content✏️ Label.Font.Pressed"] = new FontAttributes{Family = "SF Pro Text", Size = 15, Weight = (Weight)500 },
			["Content✏️ Label.Color.Pressed"] = new Color(1f, 1f, 1f, 1f),
			["BackgroundColor.Disabled"] = new Color(0.8824f, 0.8824f, 0.8824f, 1f),
			["Content✏️ Label.Disabled"] = "Disabled",
			["Content✏️ Label.Font.Disabled"] = new FontAttributes{Family = "SF Pro Text", Size = 15, Weight = (Weight)500 },
			["Content✏️ Label.Color.Disabled"] = new Color(1f, 1f, 1f, 1f),
		};
		static string[] stateKeys = new []
		{
			"BackgroundColor",
			"Content✏️ Label",
			"Content✏️ Label.Font",
			"Content✏️ Label.Color",
		};
		protected virtual void DrawBackground (SKCanvas canvas, RectangleF rectangle)
		{
			var bgRect = new RoundedRectangle(8);
			canvas.DrawShape(bgRect, rectangle, fill: this.GetEnvironment<Color>("BackgroundColor"));
			
		}
		protected virtual void DrawContent (SKCanvas canvas, RectangleF rectangle)
		{
			{
				var tb = GetTextBlock("Content✏️ Label");
				DrawText (tb, canvas, VerticalAlignment.Center);
				
			}
			
		}
		static void MapDrawBackground (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Button view)
		{
			var typed = control as FluentButtonHandler;
			typed?.DrawBackground (canvas, dirtyRect);
		}
		static void MapDrawContent (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Button view)
		{
			var typed = control as FluentButtonHandler;
			typed?.DrawContent (canvas, dirtyRect);
		}
	}
}

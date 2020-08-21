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
		public static string[] DefaultButtonLayerDrawingOrder = new[]
		{
			SkiaEnvironmentKeys.Clip,
			"Background",
			"Content",
			SkiaEnvironmentKeys.Overlay
		};
		protected override string[] LayerDrawingOrder() => DefaultButtonLayerDrawingOrder;
		static Dictionary<string, object> stateDefaultValues = new Dictionary<string, object>
		{
			["Default.BackgroundFill"] = new Color(0f, 0.47058824f, 0.83137256f, 1f),
			["Default.Content✏️ Label"] = "Default",
			["Default.Content✏️ Label.Font"] = new FontAttributes { Family = "SF Pro Text", Size = 15, Weight = (Weight)500 },
			["Default.Content✏️ Label.Color"] = new Color(1f, 1f, 1f, 1f),
			["Pressed.BackgroundFill"] = new Color(0.16862746f, 0.53333336f, 0.84705883f, 1f),
			["Pressed.Content✏️ Label"] = "Pressed",
			["Pressed.Content✏️ Label.Font"] = new FontAttributes { Family = "SF Pro Text", Size = 15, Weight = (Weight)500 },
			["Pressed.Content✏️ Label.Color"] = new Color(1f, 1f, 1f, 1f),
			["Disabled.BackgroundFill"] = new Color(0.8824f, 0.8824f, 0.8824f, 1f),
			["Disabled.Content✏️ Title"] = "Disabled",
			["Disabled.Content✏️ Title.Font"] = new FontAttributes { Family = "SF Pro Text", Size = 15, Weight = (Weight)500 },
			["Disabled.Content✏️ Title.Color"] = new Color(1f, 1f, 1f, 1f),
		};
		static string[] stateKeys = new[]
		{
			"BackgroundFill",
			"Content✏️ Label",
			"Content✏️ Label.Font",
			"Content✏️ Label.Color",
			"Content✏️ Title",
			"Content✏️ Title.Font",
			"Content✏️ Title.Color",
		};
		protected virtual void DrawBackground(SKCanvas canvas, RectangleF rectangle)
		{
			var bgRect = new RoundedRectangle(8);
			canvas.DrawShape(bgRect, rectangle, fill: this.GetEnvironment<Color>("BackgroundFill"));

		}
		protected virtual void DrawContent(SKCanvas canvas, RectangleF rectangle)
		{
			{
				var tb = GetTextBlock("Content✏️ Label");
				DrawText(tb, canvas, VerticalAlignment.Center);

			}

		}
		static void MapDrawBackground(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Button view)
		{
			var typed = control as FluentButtonHandler;
			typed?.DrawBackground(canvas, dirtyRect);
		}
		static void MapDrawContent(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Button view)
		{
			var typed = control as FluentButtonHandler;
			typed?.DrawContent(canvas, dirtyRect);
		}
	}
}

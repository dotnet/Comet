using System;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.GraphicsControls
{
	public class FluentButtonHandler : ButtonHandler
	{
		public static DrawMapper<FluentButtonHandler, IButton> ButtonDrawMapper = new DrawMapper<FluentButtonHandler, IButton>(ViewHandler.DrawMapper)
		{
			["Background"] = MapDrawBackground,
			[nameof(IText.Text)] = MapDrawText,
		};

		public FluentButtonHandler() : base(ButtonDrawMapper, null)
		{

		}


		public static void MapDrawText(ICanvas canvas, RectangleF dirtyRect, FluentButtonHandler handler, IView view) => handler.DrawText(canvas, dirtyRect);

		private static void MapDrawBackground(ICanvas canvas, RectangleF dirtyRect, FluentButtonHandler handler, IView view)
			=> handler.DrawBackground(canvas, dirtyRect);

		static Dictionary<string, object> stateDefaultValues = new Dictionary<string, object>
		{
			["BackgroundColor"] = new Color(0f, 0.47058824f, 0.83137256f, 1f),
			["Content✏️ Label"] = "Default",
			["Content✏️ Label.Font"] = Font.OfSize("SF Pro Text",15),//, Weight = (Weight)500 },
			["Content✏️ Label.Color"] = new Color(1f, 1f, 1f, 1f),
			["BackgroundColor.Pressed"] = new Color(0.16862746f, 0.53333336f, 0.84705883f, 1f),
			["Content✏️ Label.Pressed"] = "Pressed",
			["Content✏️ Label.Font.Pressed"] = Font.OfSize("SF Pro Text",15),// Weight = (Weight)500 },
			["Content✏️ Label.Color.Pressed"] = new Color(1f, 1f, 1f, 1f),
			["BackgroundColor.Disabled"] = new Color(0.8824f, 0.8824f, 0.8824f, 1f),
			["Content✏️ Label.Disabled"] = "Disabled",
			["Content✏️ Label.Font.Disabled"] = Font.OfSize("SF Pro Text",15),// Weight = (Weight)500 },
			["Content✏️ Label.Color.Disabled"] = new Color(1f, 1f, 1f, 1f),
		};

		static string[] stateKeys = new[]
{
			"BackgroundColor",
			"Content✏️ Label",
			"Content✏️ Label.Font",
			"Content✏️ Label.Color",
		};
		static string textKey => "Content✏️ Label";
		public override string[] LayerDrawingOrder() => ViewHandler.DefaultLayerDrawingOrder;


		public virtual void DrawBackground(ICanvas canvas, RectangleF rectangle)
		{
			var bgRect = new RoundedRectangle(8);
			canvas.DrawShape(bgRect, rectangle, fill: VirtualView.BackgroundColor ?? GetValueForState<Color>("BackgroundColor"));
		}

		public virtual void DrawText(ICanvas canvas, RectangleF rectangle)
		{
			var t = VirtualView;
			canvas.FillColor = t.TextColor ?? GetValueForState<Color> ("Content✏️ Label.Color");
			var font = t.Font.IsDefault ? GetValueForState<Font>("Content✏️ Label.Font") : t.Font;

			canvas.FontName = font.FontFamily;
			canvas.FontSize = (float)font.FontSize;

			var horizontal = Microsoft.Maui.Graphics.HorizontalAlignment.Center;
			canvas.DrawString(t.Text, rectangle, horizontalAlignment: horizontal, verticalAlignment: Microsoft.Maui.Graphics.VerticalAlignment.Center);

		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var font = VirtualView.Font.IsDefault ? GetValueForState<Font>("Content✏️ Label.Font") : VirtualView.Font;
			var size = GraphicsPlatform.CurrentService.GetStringSize(VirtualView.Text, font.FontFamily, (float)font.FontSize);
			return new Size(Math.Min(size.Width, widthConstraint), Math.Min(size.Height, heightConstraint));
		}

		public T GetValueForState<T>(string key)
		{
			if (key == textKey)
				return (T)(object)VirtualView.Text;
			var state = CurrentState switch
			{
				ControlState.Pressed => ".Pressed",
				ControlState.Disabled => ".Disabled",
				_ => "",
			};
			//var isOnText = currentState ? "On" : "Off";
			//var isDisabled = state == ControlState.Disabled ? " (disabled)" : "";
			var newKey = $"{key}{state}";
			if (stateDefaultValues.TryGetValue(newKey, out var r) && r is T t)
				return t;
			return default;
		}
	}
}

//using System.Collections.Generic;
//using Microsoft.Maui;
//using Microsoft.Maui.Graphics;
//using GHorizontalAlignment = Microsoft.Maui.Graphics.HorizontalAlignment;
//using GVerticalAlignment = Microsoft.Maui.Graphics.VerticalAlignment;

//namespace Comet.GraphicsControls
//{
//	public class CupertinoSliderHandler : SliderHandler
//	{
//		public static DrawMapper<CupertinoSliderHandler, ISlider> SliderDrawMapper = new DrawMapper<CupertinoSliderHandler, ISlider>(ViewHandler.DrawMapper)
//		{
//			["Background"] = MapDrawBackground,
//			["TrackProgress"] = MapDrawTrackProgress,
//			["Thumb"] = MapDrawThumb,
//			["Text"] = MapDrawText
//		};

//		public CupertinoSliderHandler() : base(SliderDrawMapper, null)
//		{

//		}

//		static readonly Dictionary<string, object> stateDefaultValues = new Dictionary<string, object>
//		{
//			["TextSize"] = 36f
//		};

//		public override string[] LayerDrawingOrder() => ViewHandler.DefaultLayerDrawingOrder;

//		public static void MapDrawBackground(ICanvas canvas, RectangleF dirtyRect, CupertinoSliderHandler handler, IView view) => handler.DrawBackground(canvas, dirtyRect);

//		public static void MapDrawTrackProgress(ICanvas canvas, RectangleF dirtyRect, CupertinoSliderHandler handler, IView view) => handler.DrawTrackProgress(canvas, dirtyRect);

//		public static void MapDrawThumb(ICanvas canvas, RectangleF dirtyRect, CupertinoSliderHandler handler, IView view) => handler.DrawThumb(canvas, dirtyRect);

//		public static void MapDrawText(ICanvas canvas, RectangleF dirtyRect, CupertinoSliderHandler handler, IView view) => handler.DrawText(canvas, dirtyRect);

//		public virtual void DrawBackground(ICanvas canvas, RectangleF dirtyRect)
//		{
//			canvas.SaveState();

//			var slider = VirtualView;

//			canvas.FillColor = slider.MaximumTrackColor;

//			var x = dirtyRect.X;

//			stateDefaultValues.TryGetValue("TextSize", out var textSize);
//			var width = dirtyRect.Width - (float)textSize;
//			var height = 4;

//			var y = (float)((slider.Height - height) / 2);

//			canvas.FillRoundedRectangle(x, y, width, height, 0);

//			canvas.RestoreState();
//		}

//		public virtual void DrawTrackProgress(ICanvas canvas, RectangleF dirtyRect)
//		{
//			canvas.SaveState();

//			var slider = VirtualView;

//			canvas.FillColor = slider.MinimumTrackColor;

//			var x = dirtyRect.X;

//			var value = ((double)slider.Value).Clamp(0, 1);

//			stateDefaultValues.TryGetValue("TextSize", out var textSize);
//			var width = (float)((dirtyRect.Width - (float)textSize) * value);

//			var height = 4;

//			var y = (float)((slider.Height - height) / 2);

//			canvas.FillRoundedRectangle(x, y, width, height, 0);

//			canvas.RestoreState();
//		}

//		public virtual void DrawThumb(ICanvas canvas, RectangleF dirtyRect)
//		{
//			canvas.SaveState();

//			var slider = VirtualView;

//			var size = 16f;
//			var strokeWidth = 2f;

//			canvas.StrokeColor = slider.ThumbColor;

//			canvas.StrokeSize = strokeWidth;

//			var value = ((double)slider.Value).Clamp(0, 1);
//			stateDefaultValues.TryGetValue("TextSize", out var textSize);
//			var x = (float)(((dirtyRect.Width - (float)textSize) * value) - (size / 2));

//			if (x <= strokeWidth)
//				x = strokeWidth;

//			if (x >= dirtyRect.Width - (size + strokeWidth))
//				x = dirtyRect.Width - (size + strokeWidth);

//			var y = (float)((slider.Height - size) / 2);

//			canvas.FillColor = Colors.Black;

//			canvas.FillEllipse(x, y, size, size);
//			canvas.DrawEllipse(x, y, size, size);

//			canvas.RestoreState();
//		}

//		public virtual void DrawText(ICanvas canvas, RectangleF dirtyRect)
//		{
//			canvas.SaveState();

//			var slider = VirtualView;

//			canvas.FontColor = Colors.Black;
//			canvas.FontSize = 14f;

//			var height = dirtyRect.Height;
//			var width = dirtyRect.Width;

//			var margin = 6;
//			stateDefaultValues.TryGetValue("TextSize", out var textSize);
//			var x = (float)(width - (float)textSize + margin);
//			var y = 2;

//			canvas.SetToBoldSystemFont();

//			var value = ((double)slider.Value).Clamp(0, 1).ToString("####0.00");

//			canvas.DrawString(value, x, y, (float)textSize, height, GHorizontalAlignment.Left, GVerticalAlignment.Center);

//			canvas.RestoreState();
//		}
//	}
//}
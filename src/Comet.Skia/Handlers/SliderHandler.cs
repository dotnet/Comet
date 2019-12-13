using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Comet.Internal;
using SkiaSharp;

namespace Comet.Skia
{
	public class SliderHandler : SkiaAbstractControlHandler<Slider>
	{
		public static DrawMapper<Slider> SliderDrawMapper = new DrawMapper<Slider>(SkiaControl.DrawMapper)
		{
			[SkiaEnvironmentKeys.Slider.Layers.Track] = DrawTrack,
			[SkiaEnvironmentKeys.Slider.Layers.Progress] = DrawProgress,
			[SkiaEnvironmentKeys.Slider.Layers.Thumb] = DrawThumb,
		};


		static float hPadding = 8;
		static float minHPadding = 10;
		static float vPadding = 10;

		public SliderHandler() : base(SliderDrawMapper, null)
		{
			Console.WriteLine("HELLO!!!!!!!"); 
		}
		//public override SizeF Measure(SizeF availableSize) => availableSize;

		public override string AccessibilityText() => (TypedVirtualView?.Value?.CurrentValue ?? 0).ToString("P");

		public static string[] DefaultSliderLayerDrawingOrder =
			DefaultLayerDrawingOrder.ToList().InsertAfter(new string[] {
				SkiaEnvironmentKeys.Slider.Layers.Track,
				SkiaEnvironmentKeys.Slider.Layers.Progress,
				SkiaEnvironmentKeys.Slider.Layers.Thumb,
				}, SkiaEnvironmentKeys.Text).ToArray();

		protected override string[] LayerDrawingOrder()
			=> DefaultSliderLayerDrawingOrder;
		const float defaultHeight = 4f;
		public virtual void DrawTrack(SKCanvas canvas, float hSpace, RectangleF rectangle)
		{ 
			return;  
			var paint = new SKPaint();
			SKColor fillColor = ColorFromArgb(255, 0, 0, 0);
			SKColor fillColor11 = ColorFromArgb(255, 3, 218, 197);

			var width = rectangle.Width - (hSpace * 2);
			var height = defaultHeight;
			var y = rectangle.Y + (height / 2);
			var highlightRect = new RectangleF(hSpace, y, width, height).ToSKRect();
			//var highlightRect = rectangle.ApplyPadding(new Thickness(hSpace, vSpace)).ToSKRect();
			var highlightPath = new SKPath();
			highlightPath.Reset();
			highlightPath.AddRoundedRect(highlightRect, 1f, 1f, SKPathDirection.Clockwise);

			paint.Reset();
			paint.IsAntialias = true;
			paint.Style = SKPaintStyle.Fill;
			paint.Color = (SKColor)fillColor11;
			canvas.DrawPath(highlightPath, paint);
			canvas.Restore();

		}

		public virtual void DrawProgress(SKCanvas canvas, float progress, float hSpace, RectangleF rectangle)
		{
			return;
			var paint = new SKPaint();
			SKColor fillColor = ColorFromArgb(255, 0, 0, 0);
			SKColor fillColor11 = ColorFromArgb(255, 3, 218, 197);
			var width = rectangle.Width - (hSpace * 2);
			var height = defaultHeight;
			var y = rectangle.Y + (height / 2);
			var highlightRect = new RectangleF(hSpace, y, width, height).ToSKRect();
			var trackRect = highlightRect;
			SKPath trackPath = new SKPath();
			trackPath.Reset();
			trackPath.AddRoundedRect(trackRect, 1f, 1f, SKPathDirection.Clockwise);
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			paint.Reset();
			paint.IsAntialias = true;
			paint.Style = SKPaintStyle.Fill;
			paint.Color = (SKColor)fillColor11;
			canvas.DrawPath(trackPath, paint);
		}

		public virtual void DrawThumb(SKCanvas canvas, float progress, RectangleF rectangle)
		{

			var paint = new SKPaint();
			SKColor fillColor = ColorFromArgb(255, 0, 0, 0);
			SKColor fillColor11 = ColorFromArgb(255, 3, 218, 197);
			var knobRect = new SKRect(117.58f, 3f, 129.58f, 15f);

			SKPath knobPath = new SKPath();
			knobPath.Reset();
			knobPath.AddOval(knobRect, SKPathDirection.Clockwise);

			paint.Reset();
			paint.IsAntialias = true;
			paint.Style = SKPaintStyle.Fill;
			paint.Color = (SKColor)fillColor11;
			canvas.DrawPath(knobPath, paint);
		}


		public static void DrawTrack(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Slider view)
		{
			var slider = control as SliderHandler;
			slider?.DrawTrack(canvas, hPadding, dirtyRect);
		}
		public static void DrawThumb(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Slider view)
		{
			var slider = control as SliderHandler;
			var progress = view?.GetPercent() ?? 0;
			slider?.DrawThumb(canvas, progress, dirtyRect);
		}
		public static void DrawProgress(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Slider view)
		{
			var slider = control as SliderHandler;
			var progress = view?.GetPercent() ?? 0;
			slider?.DrawProgress(canvas, progress, hPadding, dirtyRect);
		}



		public static SKColor ColorFromArgb(byte alpha, byte red, byte green, byte blue)
		{
			return new SKColor(red, blue, green, alpha);
		}
		public static SKPaint PaintWithAlpha(byte alpha)
		{
			return new SKPaint { Color = new SKColor(255, 255, 255, alpha) };
		}
	}
}

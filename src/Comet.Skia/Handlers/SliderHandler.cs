using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Comet.Internal;
using SkiaSharp;
using Comet;

namespace Comet.Skia
{
	public class SliderHandler : SkiaAbstractControlHandler<Slider>
	{
		public static new readonly PropertyMapper<Slider> Mapper = new PropertyMapper<Slider>(SkiaControl.Mapper)
		{
			[nameof(Slider.Value)] = Redraw,
		};
		public static DrawMapper<Slider> SliderDrawMapper = new DrawMapper<Slider>(SkiaControl.DrawMapper)
		{
			[SkiaEnvironmentKeys.Slider.Layers.Track] = DrawTrack,
			[SkiaEnvironmentKeys.Slider.Layers.Progress] = DrawProgress,
			[SkiaEnvironmentKeys.Slider.Layers.Thumb] = DrawThumb,
		};


		static float hPadding = 8;
		static float minHPadding = 10;
		static float vPadding = 10;
		static float touchSize = 44f;

		public SliderHandler() : base(SliderDrawMapper, Mapper)
		{

		}

		public override string AccessibilityText() => (TypedVirtualView?.Value?.CurrentValue ?? 0).ToString("P");

		public static string[] DefaultSliderLayerDrawingOrder =
			DefaultLayerDrawingOrder.ToList().InsertAfter(new string[] {
				SkiaEnvironmentKeys.Slider.Layers.Track,
				SkiaEnvironmentKeys.Slider.Layers.Progress,
				SkiaEnvironmentKeys.Slider.Layers.Thumb,
				}, SkiaEnvironmentKeys.Text).ToArray();
		bool isTracking = false;
		public override bool StartInteraction(PointF[] points)
		{
			isTracking = TouchTargetRect.Contains(points);
			return base.StartInteraction(points);
		}

		public override void DragInteraction(PointF[] points)
		{
			base.DragInteraction(points);

			if (!isTracking)
				return;
			//Only track the first point;
			var point = points[0];
			var percent = (point.X - TrackRect.X) / TrackRect.Width;
			TypedVirtualView.PercentChanged(percent);
		}
		public override void CancelInteraction()
		{
			isTracking = false;

			base.CancelInteraction();
		}
		public override void EndInteraction(PointF[] points, bool inside)
		{
			isTracking = false;
			base.EndInteraction(points, inside);
		}

		RectangleF TrackRect = new RectangleF();
		RectangleF TouchTargetRect = new RectangleF(0, 0, touchSize,touchSize);
		protected override string[] LayerDrawingOrder()
			=> DefaultSliderLayerDrawingOrder;
		const float defaultHeight = 2f;
		public virtual void DrawTrack(SKCanvas canvas, float hSpace, RectangleF rectangle)
		{
			var paint = new SKPaint();
			SKColor fillColor11 = ColorFromArgb(97, 3, 218, 197);

			TrackRect.Width = rectangle.Width - (hSpace * 2);
			TrackRect.Height = defaultHeight;
			TrackRect.Y = rectangle.Y + ((rectangle.Height - TrackRect.Height) / 2);
			TrackRect.X = hSpace;
			var highlightRect = TrackRect.ToSKRect();
			//var highlightRect = rectangle.ApplyPadding(new Thickness(hSpace, vSpace)).ToSKRect();
			var highlightPath = new SKPath();
			highlightPath.Reset();
			highlightPath.AddRoundedRect(highlightRect, 1f, 1f, SKPathDirection.Clockwise);

			paint.Reset();
			paint.IsAntialias = true;
			paint.Style = SKPaintStyle.Fill;
			paint.Color = (SKColor)fillColor11;
			canvas.DrawPath(highlightPath, paint);

		}

		public virtual void DrawProgress(SKCanvas canvas, float progress, float hSpace, RectangleF rectangle)
		{
			var paint = new SKPaint();
			SKColor fillColor = ColorFromArgb(255, 0, 0, 0);
			SKColor fillColor11 = ColorFromArgb(255, 3, 218, 197);
			var width = (rectangle.Width - (hSpace * 2)) * progress;
			var height = defaultHeight;
			var y = rectangle.Y + ((rectangle.Height - height) / 2);
			var highlightRect = new RectangleF(hSpace, y, width, height).ToSKRect();

			var trackRect = highlightRect;
			SKPath trackPath = new SKPath();
			trackPath.Reset();
			trackPath.AddRoundedRect(trackRect, 1f, 1f, SKPathDirection.Clockwise);
			paint.Reset();
			paint.IsAntialias = true;
			paint.Style = SKPaintStyle.Fill;
			paint.Color = (SKColor)fillColor11;
			canvas.DrawPath(trackPath, paint);
		}

		RectangleF ThumbRect = new RectangleF(0, 0, defaultThumbHeight, defaultThumbHeight);
		const float defaultThumbHeight = 12f;
		public virtual void DrawThumb(SKCanvas canvas, float progress, float hSpace, RectangleF rectangle)
		{

			var paint = new SKPaint();
			SKColor fillColor11 = ColorFromArgb(255, 3, 218, 197);

			ThumbRect.X = (rectangle.Width - (hSpace * 2) - ThumbRect.Width) * progress + hSpace;
			ThumbRect.Y = rectangle.Y + (rectangle.Height - ThumbRect.Height) / 2;
			TouchTargetRect.Center(ThumbRect.Center());
			var knobRect = ThumbRect.ToSKRect();

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
			slider?.DrawThumb(canvas, progress, hPadding, dirtyRect);
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

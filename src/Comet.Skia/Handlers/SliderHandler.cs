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

		static Color defaultThumbColor = Color.FromBytes(3, 218, 197, 255);
		static Color defaultTrackColor = Color.FromBytes(3, 218, 197, 97);
		static Color defaultProgressColor = Color.FromBytes(3, 218, 197, 255);

		static float hPadding = 8;
		static float touchSize = 44f;

		public SliderHandler() : base(SliderDrawMapper, Mapper)
		{

		}

		public override string AccessibilityText() => (TypedVirtualView?.GetPercent() ?? 0).ToString("P");

		protected override string[] LayerDrawingOrder()
			=> DefaultSliderLayerDrawingOrder;

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
			this.Invalidate();
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
		RectangleF TouchTargetRect = new RectangleF(0, 0, touchSize, touchSize);
		const float defaultHeight = 2f;
		public virtual void DrawTrack(SKCanvas canvas, float hSpace, RectangleF rectangle)
		{
			using var paint = new SKPaint();
			var fillColor = TypedVirtualView.GetTrackColor(defaultTrackColor, CurrentState).ToSKColor();

			TrackRect.Width = rectangle.Width - (hSpace * 2);
			TrackRect.Height = defaultHeight;
			TrackRect.Y = rectangle.Y + ((rectangle.Height - TrackRect.Height) / 2);
			TrackRect.X = hSpace;
			var highlightRect = TrackRect.ToSKRect();
			//var highlightRect = rectangle.ApplyPadding(new Thickness(hSpace, vSpace)).ToSKRect();
			using var highlightPath = new SKPath();
			highlightPath.Reset();
			highlightPath.AddRoundRect(highlightRect, 1f, 1f, SKPathDirection.Clockwise);

			paint.Reset();
			paint.IsAntialias = true;
			paint.Style = SKPaintStyle.Fill;
			paint.Color = fillColor;
			canvas.DrawPath(highlightPath, paint);

		}

		public virtual void DrawProgress(SKCanvas canvas, float progress, float hSpace, RectangleF rectangle)
		{
			using var paint = new SKPaint();

			var fillColor = TypedVirtualView.GetProgressColor(defaultProgressColor, CurrentState).ToSKColor();
			var width = (rectangle.Width - (hSpace * 2)) * progress;
			var height = defaultHeight;
			var y = rectangle.Y + ((rectangle.Height - height) / 2);
			var highlightRect = new RectangleF(hSpace, y, width, height).ToSKRect();

			var trackRect = highlightRect;
			using var trackPath = new SKPath();
			trackPath.Reset();
			trackPath.AddRoundRect(trackRect, 1f, 1f, SKPathDirection.Clockwise);
			paint.Reset();
			paint.IsAntialias = true;
			paint.Style = SKPaintStyle.Fill;
			paint.Color = fillColor;
			canvas.DrawPath(trackPath, paint);
		}

		RectangleF ThumbRect = new RectangleF(0, 0, defaultThumbHeight, defaultThumbHeight);
		const float defaultThumbHeight = 12f;
		public virtual void DrawThumb(SKCanvas canvas, float progress, float hSpace, RectangleF rectangle)
		{
			using var paint = new SKPaint();

			var fillColor = TypedVirtualView.GetThumbColor(defaultThumbColor, CurrentState).ToSKColor();
			ThumbRect.X = (rectangle.Width - (hSpace * 2) - ThumbRect.Width) * progress + hSpace;
			ThumbRect.Y = rectangle.Y + (rectangle.Height - ThumbRect.Height) / 2;
			TouchTargetRect.Center(ThumbRect.Center());
			var knobRect = ThumbRect.ToSKRect();

			using var knobPath = new SKPath();
			knobPath.Reset();
			knobPath.AddOval(knobRect, SKPathDirection.Clockwise);

			paint.Reset();
			paint.IsAntialias = true;
			paint.Style = SKPaintStyle.Fill;
			paint.Color = fillColor;
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
	}
}

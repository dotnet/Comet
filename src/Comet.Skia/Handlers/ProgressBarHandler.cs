using System;
using System.Drawing;
using System.Linq;
using Comet.Internal;
using SkiaSharp;

namespace Comet.Skia
{
	public class ProgressBarHandler : SkiaAbstractControlHandler<ProgressBar>
	{
		static float hPadding = 8;

		static Color defaultTrackColor = Color.FromBytes(3, 218, 197, 97);
		static Color defaultProgressColor = Color.FromBytes(3, 218, 197, 255);

		public static new readonly PropertyMapper<ProgressBar> Mapper = new PropertyMapper<ProgressBar>(SkiaControl.Mapper)
		{
			[nameof(ProgressBar.Value)] = Redraw,
		};
		public static DrawMapper<ProgressBar> ProgressBarDrawMapper = new DrawMapper<ProgressBar>(SkiaControl.DrawMapper)
		{
			[SkiaEnvironmentKeys.Slider.Layers.Track] = DrawTrack,
			[SkiaEnvironmentKeys.Slider.Layers.Progress] = DrawProgress,
		};

		public ProgressBarHandler() : base(ProgressBarDrawMapper, Mapper) { }

		protected override string[] LayerDrawingOrder()
			=> DefaultSliderLayerDrawingOrder;

		public static string[] DefaultSliderLayerDrawingOrder =
			DefaultLayerDrawingOrder.ToList().InsertAfter(new string[] {
				SkiaEnvironmentKeys.Slider.Layers.Track,
				SkiaEnvironmentKeys.Slider.Layers.Progress,
				}, SkiaEnvironmentKeys.Text).ToArray();

		RectangleF TrackRect = new RectangleF();
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


		public static void DrawProgress(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, ProgressBar view)
		{
			var slider = control as ProgressBarHandler;
			var progress = view?.Value?.CurrentValue ?? 0;
			slider?.DrawProgress(canvas, progress, hPadding, dirtyRect);
		}


		public static void DrawTrack(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, ProgressBar view)
		{
			var slider = control as ProgressBarHandler;
			slider?.DrawTrack(canvas, hPadding, dirtyRect);
		}

		public override string AccessibilityText() => (TypedVirtualView?.Value?.CurrentValue ?? 0).ToString("P");
	}
}

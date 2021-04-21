using System;
using System.Collections.Generic;
using System.Linq;
using Comet.Handlers;
using Comet.Internal;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.GraphicsControls
{
	public class SliderHandler : GraphicsControlHandler<ISliderDrawable, ISlider>
	{
		public static PropertyMapper<ISlider> PropertyMapper = new(ViewHandler.Mapper)
		{
			Actions = {
				[nameof(ISlider.Value)] = ViewHandler.MapInvalidate,
				[nameof(ISlider.Value)] = ViewHandler.MapInvalidate,
				[nameof(ISlider.ThumbColor)] = ViewHandler.MapInvalidate,
				[nameof(ISlider.MaximumTrackColor)] = ViewHandler.MapInvalidate,
				[nameof(ISlider.MinimumTrackColor)] = ViewHandler.MapInvalidate,
			}
		};

		public static DrawMapper<ISliderDrawable, ISlider> DrawMapper = new(ViewHandler.DrawMapper)
		{
			["TrackProgress"] = MapDrawTrackProgress,
			["Thumb"] = MapDrawThumb,
			["Text"] = MapDrawText
		};


		public SliderHandler() : base(DrawMapper, PropertyMapper)
		{

		}

		static float hPadding = 8;
		static float touchSize = 44f;

		private static void MapDrawThumb(ICanvas canvas, RectangleF dirtyRect, ISliderDrawable drawable, ISlider view)
			=> drawable.DrawThumb(canvas, dirtyRect, view);

		public static void MapDrawTrackProgress(ICanvas canvas, RectangleF dirtyRect, ISliderDrawable drawable, ISlider view)
			=> drawable.DrawTrackProgress(canvas, dirtyRect, view);

		private static void MapDrawText(ICanvas canvas, RectangleF dirtyRect, ISliderDrawable drawable, ISlider view)
			=> drawable.DrawText(canvas, dirtyRect, view);

		bool isTracking = false;

		public override bool StartInteraction(PointF[] points)
		{
			isTracking = Drawable.TouchTargetRect.Contains(points);
			return base.StartInteraction(points);
		}

		public override void DragInteraction(PointF[] points)
		{
			if (!isTracking)
				return;

			VirtualView?.DragStarted();
			//Only track the first point;
			var point = points[0];
			var TrackRect = Drawable.TrackRect;
			var progress = (point.X - TrackRect.X) / TrackRect.Width;


			var min = VirtualView.Minimum;
			var max = VirtualView.Maximum;

			var value = min + (max - min) * progress;

			VirtualView.Value = value;

			base.DragInteraction(points);
		}

		public override void EndInteraction(PointF[] points, bool inside)
		{

			isTracking = false;
			VirtualView?.DragCompleted();

			base.EndInteraction(points, inside);
		}
		public override void CancelInteraction()
		{
			isTracking = false;
			base.CancelInteraction();
		}
		public static string[] DefaultSliderLayerDrawingOrder =
		ViewHandler.DefaultLayerDrawingOrder.ToList().InsertAfter(new string[] {
				"TrackProgress",
				"Thumb",
			}, "Text").ToArray(); 
		protected override ISliderDrawable CreateDrawable() => new MaterialSliderDrawable();
		public override string[] LayerDrawingOrder() => DefaultSliderLayerDrawingOrder;

	}
}
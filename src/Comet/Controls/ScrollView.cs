using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;

namespace Comet
{
	public class ScrollView : ContentView, IEnumerable, IScrollView
	{
		public ScrollView(Orientation orientation = Orientation.Vertical)
		{
			Orientation = orientation;
		}

		public Orientation Orientation { get; }

		ScrollBarVisibility IScrollView.HorizontalScrollBarVisibility => this.GetPropertyValue<ScrollBarVisibility?>() ?? ScrollBarVisibility.Default;

		ScrollBarVisibility IScrollView.VerticalScrollBarVisibility => this.GetPropertyValue<ScrollBarVisibility?>() ?? ScrollBarVisibility.Default;

		ScrollOrientation IScrollView.Orientation => Orientation == Orientation.Horizontal ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical;

		Size IScrollView.ContentSize => Content?.MeasuredSize ?? Size.Zero;

		double IScrollView.HorizontalOffset { get; set; }
		double IScrollView.VerticalOffset { get; set; }

		public override Size GetDesiredSize(Size availableSize)
		{

			var frameConstraints = this.GetFrameConstraints();
			
			var contentMeasureSize = availableSize;

			if (frameConstraints?.Width > 0)
				contentMeasureSize.Width = frameConstraints.Width.Value;
			if(frameConstraints?.Height > 0)
				contentMeasureSize.Height = frameConstraints.Height.Value;

			if (Orientation == Orientation.Vertical)
				contentMeasureSize.Height = double.PositiveInfinity;
			else
				contentMeasureSize.Width = double.PositiveInfinity;
			
			if (Content != null)
			{
				var contentSize = Content.MeasuredSize;
				if (!Content.MeasurementValid)
				{
					contentSize = Content.Measure(contentMeasureSize.Width, contentMeasureSize.Height);
					Content.MeasuredSize = contentSize;
				}
				MeasurementValid = true;
				return MeasuredSize = new Size(
					Math.Min(availableSize.Width, contentSize.Width),
					Math.Min(availableSize.Height, contentSize.Height));
				
			}
			if (frameConstraints?.Height > 0 && frameConstraints?.Width > 0)
				return MeasuredSize = new Size(frameConstraints.Width.Value, frameConstraints.Height.Value);
			return MeasuredSize = availableSize;
		}
		public override void LayoutSubviews(Rectangle frame)
		{
			this.Frame = frame;

			Content.SetFrameFromPlatformView(frame,LayoutAlignment.Start,	LayoutAlignment.Start);
			if (Content?.BuiltView != null)
				Content.BuiltView.LayoutSubviews(frame);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				Content?.Dispose();
			base.Dispose(disposing);
		}

		void IScrollView.RequestScrollTo(double horizontalOffset, double verticalOffset, bool instant) { }
		void IScrollView.ScrollFinished() { }
	}
}

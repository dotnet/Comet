using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;

namespace Comet
{
	public class ScrollView : ContentView, IEnumerable, IScrollView
	{
		public ScrollView(ScrollOrientation orientation = ScrollOrientation.Vertical)
		{
			Orientation = orientation;
		}

		public ScrollOrientation Orientation { get; }

		ScrollBarVisibility IScrollView.HorizontalScrollBarVisibility => ScrollBarVisibility.Default;

		ScrollBarVisibility IScrollView.VerticalScrollBarVisibility => ScrollBarVisibility.Default;

		ScrollOrientation IScrollView.Orientation => Orientation;

		Size IScrollView.ContentSize => Content.MeasuredSize;

		double IScrollView.HorizontalOffset { get; set; }
		double IScrollView.VerticalOffset { get; set; }

		public override Size GetDesiredSize(Size availableSize)
		{
			var contentMeasureSize = availableSize;
			if (Orientation == ScrollOrientation.Vertical)
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
					Content.MeasurementValid = true;
				}
				MeasurementValid = true;
				return MeasuredSize = new Size(
					Math.Min(availableSize.Width, contentSize.Width),
					Math.Min(availableSize.Height, contentSize.Height));

			}
			return MeasuredSize = availableSize;
		}
		public override void LayoutSubviews(Rectangle frame)
		{
			this.Frame = frame;
			if (Content != null)
			{
				var margin = Content.GetMargin();
				frame = new Rectangle(Point.Zero, Content.MeasuredSize);
				var bounds = new Rectangle(
					frame.Left + margin.Left,
					frame.Top + margin.Top,
					frame.Width - margin.HorizontalThickness,
					frame.Height - margin.VerticalThickness);
				Content.Frame = bounds;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				Content?.Dispose();
			base.Dispose(disposing);
		}

		void IScrollView.RequestScrollTo(double horizontalOffset, double verticalOffset, bool instant) {

		}
		void IScrollView.ScrollFinished() {

		}
	}
}

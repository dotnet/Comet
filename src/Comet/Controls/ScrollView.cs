using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;

namespace Comet
{
	public class ScrollView : ContentView, IEnumerable
	{
		public ScrollView(Orientation orientation = Orientation.Vertical)
		{
			Orientation = orientation;
		}

		public Orientation Orientation { get; }
		
		public override Size GetDesiredSize(Size availableSize)
		{
			var contentMeasureSize = availableSize;
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
			Content?.LayoutSubviews(frame);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				Content?.Dispose();
			base.Dispose(disposing);
		}
	}
}

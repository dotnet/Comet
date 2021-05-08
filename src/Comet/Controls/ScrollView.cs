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
			var intrinsicSize = base.GetDesiredSize(availableSize);
			if (Orientation == Orientation.Horizontal)
			{
				if (Content != null)
				{
					var contentSize = Content.MeasuredSize;
					if (!Content.MeasurementValid)
					{
						contentSize = Content.Measure(availableSize.Width,availableSize.Height);
						Content.MeasuredSize = contentSize;
						Content.MeasurementValid = true;
					}

					intrinsicSize.Height = contentSize.Height;
				}
			}

			return intrinsicSize;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				Content?.Dispose();
			base.Dispose(disposing);
		}
	}
}

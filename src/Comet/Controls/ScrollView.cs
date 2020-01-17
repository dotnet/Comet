using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Comet
{
	public class ScrollView : View, IEnumerable
	{
		public ScrollView(Orientation orientation = Orientation.Vertical)
		{
			Orientation = orientation;
		}

		public Orientation Orientation { get; }

		public View View { get; internal set; }
		public void Add(View view)
		{
			if (view == null)
				return;
			if (View != null)
				throw new Exception("You can only add one view to the ScrollView, Try wrapping in a Stack");
			View = view;
			view.Parent = this;
			view.Navigation = this.Navigation;
		}

		public IEnumerator GetEnumerator() => new View[] { View }.GetEnumerator();
		protected override void OnParentChange(View parent)
		{
			base.OnParentChange(parent);
			if (View != null)
			{
				View.Parent = this.Parent;
				View.Navigation = this.Parent?.Navigation;
			}
		}

		public override SizeF GetIntrinsicSize(SizeF availableSize)
		{
			var intrinsicSize = base.GetIntrinsicSize(availableSize);
			if (Orientation == Orientation.Horizontal)
			{
				if (View != null)
				{
					var contentSize = View.MeasuredSize;
					if (!View.MeasurementValid)
					{
						contentSize = View.Measure(availableSize);
						View.MeasuredSize = contentSize;
						View.MeasurementValid = true;
					}

					intrinsicSize.Height = contentSize.Height;
				}
			}

			return intrinsicSize;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				View?.Dispose();
			base.Dispose(disposing);
		}
	}
}

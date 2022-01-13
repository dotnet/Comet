using System;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Primitives;

namespace Comet.Layout
{
	public class ZStackLayoutManager : ILayoutManager
	{
		public ZStackLayoutManager(ILayout layout) => this.layout = layout;

		ILayout layout;

		public Size Measure(double wConstraint, double hConstraint) {

			var v = layout as View;
			var frameConstraints = v?.GetFrameConstraints();


			var layoutVerticalSizing = layout.VerticalLayoutAlignment;
			var layoutHorizontalSizing = layout.HorizontalLayoutAlignment;

			double widthConstraint = frameConstraints?.Width > 0 ? frameConstraints.Width.Value : wConstraint;
			double heightConstraint = frameConstraints?.Height > 0 ? frameConstraints.Height.Value : hConstraint;

			var padding = layout.Padding;
			widthConstraint -= padding.HorizontalThickness;
			heightConstraint -= padding.VerticalThickness;

			Size measuredSize = new ();
			foreach(var c in layout)
			{
				var s = c.Measure(widthConstraint, heightConstraint);
				measuredSize.Height = Math.Max(measuredSize.Height, s.Height);
				measuredSize.Width = Math.Max(measuredSize.Width, s.Width);
			};
			measuredSize.Height += padding.VerticalThickness;
			measuredSize.Width += padding.HorizontalThickness;

			if (layoutVerticalSizing == LayoutAlignment.Fill && !double.IsInfinity(hConstraint))
				measuredSize.Height = hConstraint;
			if (layoutHorizontalSizing == LayoutAlignment.Fill && !double.IsInfinity(wConstraint))
				measuredSize.Width = wConstraint;

			if (frameConstraints?.Height > 0 && frameConstraints?.Width > 0)
				return new Size(frameConstraints.Width.Value, frameConstraints.Height.Value);

			return measuredSize;
		}

		public Size ArrangeChildren(Rectangle bounds)
		{
			var padding = layout.Padding;
			var b = bounds.ApplyPadding(padding);
			foreach (var v in layout)
			{
				if (v is View cv)
					cv.SetFrameFromNativeView(b);
				else
					v.Arrange(b);
			}
			return bounds.Size;
		}


	}
}

using System;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Comet.Layout
{
	public class ZStackLayoutManager : ILayoutManager
	{
		public ZStackLayoutManager(ILayout layout) => this.layout = layout;

		ILayout layout;

		public Size Measure(double widthConstraint, double heightConstraint) {
			Size measuredSize = new ();
			foreach(var c in layout)
			{
				var s = c.Measure(widthConstraint, heightConstraint);
				measuredSize.Height = Math.Max(measuredSize.Height, s.Height);
				measuredSize.Width = Math.Max(measuredSize.Width, s.Width);
			};
			return measuredSize;
		}

		public Size ArrangeChildren(Rectangle bounds)
		{
			foreach (var v in layout)
			{
				v.Arrange(bounds);
			}
			return bounds.Size;
		}


	}
}

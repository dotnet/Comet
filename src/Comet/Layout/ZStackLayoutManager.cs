
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Comet.Layout
{
	public class ZStackLayoutManager : ILayoutManager
	{
		public ZStackLayoutManager(ILayout layout) => this.layout = layout;

		ILayout layout;

		public Size Measure(double widthConstraint, double heightConstraint) => new(widthConstraint, heightConstraint);
		public void ArrangeChildren(Rectangle bounds)
		{
			foreach (var v in layout.Children)
			{
				v.Arrange(bounds);
			}
		}
	}
}

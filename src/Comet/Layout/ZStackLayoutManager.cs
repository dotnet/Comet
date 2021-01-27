using System.Graphics;
using Xamarin.Platform;
using Xamarin.Platform.Layouts;

namespace Comet.Layout
{
	public class ZStackLayoutManager : ILayoutManager
	{
		public ZStackLayoutManager(ILayout layout) => this.layout = layout;

		ILayout layout;

		public Size Measure(double widthConstraint, double heightConstraint) => new(widthConstraint, heightConstraint);
		public void Arrange(Rectangle bounds)
		{
			foreach (var v in layout.Children)
			{
				v.Arrange(bounds);
			}
		}
	}
}

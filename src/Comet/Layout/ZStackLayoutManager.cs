using System.Graphics;
using Xamarin.Platform;
using Xamarin.Platform.Layouts;

namespace Comet.Layout
{
	public class ZStackLayoutManager : ILayoutManager
	{
		public ZStackLayoutManager(ILayout layout) => this.layout = layout;

		ILayout layout;

		public SizeF Measure(float widthConstraint, float heightConstraint) => new(widthConstraint, heightConstraint);
		public void Arrange(RectangleF bounds)
		{
			foreach (var v in layout.Children)
			{
				v.Arrange(bounds);
			}
		}
	}
}

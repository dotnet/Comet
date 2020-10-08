using Xamarin.Platform;
using Xamarin.Platform.Layouts;

namespace Comet
{
	public class VStack : AbstractLayout, IStackLayout
	{
		private readonly double? spacing;

		public VStack(
			HorizontalAlignment alignment = HorizontalAlignment.Center,
			double? spacing = null)
		{
			this.spacing = spacing;
		}

		int IStackLayout.Spacing => (int)(spacing ?? 0);

		public override ILayoutManager CreateLayoutManager() => new VerticalStackLayoutManager(this);
	}
}

using Comet.Layout;
using Xamarin.Platform;
using Xamarin.Platform.Layouts;

namespace Comet
{
	public class VStack : AbstractLayout, IStackLayout
	{
		private readonly float? spacing;

		public VStack(
			HorizontalAlignment alignment = HorizontalAlignment.Center,
			float? spacing = null) 
		{
			this.spacing = spacing;
		}

		int IStackLayout.Spacing => (int)(spacing ?? -1);

		protected override ILayoutManager CreateLayoutManager() => new VerticalStackLayoutManager(this);
	}
}

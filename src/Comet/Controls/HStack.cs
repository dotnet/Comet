using Comet.Layout;
using Xamarin.Platform;
using Xamarin.Platform.Layouts;

namespace Comet
{
	public class HStack : AbstractLayout, IStackLayout
	{
		private readonly float? spacing;

		public HStack(
			VerticalAlignment alignment = VerticalAlignment.Center,
			float? spacing = null) : base()
		{
			this.spacing = spacing;
		}

		int IStackLayout.Spacing =>(int)(spacing ?? -1);

		protected override ILayoutManager CreateLayoutManager() => new HorizontalStackLayoutManager(this);
	}
}

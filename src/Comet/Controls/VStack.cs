using Comet.Layout;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;

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

		int IStackLayout.Spacing => (int)(spacing ?? 6);

		protected override ILayoutManager CreateLayoutManager() => new VerticalStackLayoutManager(this);
	}
}

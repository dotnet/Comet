using Comet.Layout;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;

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

		int IStackLayout.Spacing =>(int)(spacing ?? 6);

		protected override ILayoutManager CreateLayoutManager() => new HorizontalStackLayoutManager(this);
	}
}

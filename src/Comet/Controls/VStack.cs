using Comet.Layout;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;

namespace Comet
{
	public class VStack : AbstractLayout, IStackLayout
	{
		private readonly HorizontalAlignment alignment;
		private readonly float? spacing;

		public VStack(
			HorizontalAlignment alignment = HorizontalAlignment.Center,
			float? spacing = null) 
		{
			this.alignment = alignment;
			this.spacing = spacing;
		}

		double IStackLayout.Spacing => (int)(spacing ?? 6);

		protected override ILayoutManager CreateLayoutManager() => new  VStackLayoutManager(this, alignment,spacing);
	}
}

using Comet.Layout;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;

namespace Comet
{
	public class VStack : AbstractLayout, IStackLayout
	{
		private readonly LayoutAlignment alignment;
		private readonly float? spacing;

		public VStack(
			LayoutAlignment alignment = LayoutAlignment.Center,
			float? spacing = null) 
		{
			this.alignment = alignment;
			this.spacing = spacing;
		}

		double IStackLayout.Spacing => (int)(spacing ?? 6);

		protected override ILayoutManager CreateLayoutManager() => new  VStackLayoutManager(this, alignment,spacing);
	}
}

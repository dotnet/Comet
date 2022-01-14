using Comet.Layout;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;

namespace Comet
{
	public class HStack : AbstractLayout, IStackLayout
	{
		private readonly LayoutAlignment alignment;
		private readonly float? spacing;

		public HStack(
			LayoutAlignment alignment = LayoutAlignment.Center,
			float? spacing = null) : base()
		{
			this.alignment = alignment;
			this.spacing = spacing;
		}

		double IStackLayout.Spacing =>(spacing ?? 6);

		protected override ILayoutManager CreateLayoutManager() => new HStackLayoutManager(this,alignment,spacing);
	}
}

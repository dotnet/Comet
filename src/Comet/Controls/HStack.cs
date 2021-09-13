using Comet.Layout;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;

namespace Comet
{
	public class HStack : AbstractLayout, IStackLayout
	{
		private readonly float? spacing;

		public HStack() : this(null) { }
		public HStack(float? spacing) : base()
		{
			this.spacing = spacing;
		}

		double IStackLayout.Spacing =>(spacing ?? 6);

		protected override ILayoutManager CreateLayoutManager() => new HorizontalStackLayoutManager(this);
	}
}

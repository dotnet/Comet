using Comet.Layout;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;

namespace Comet
{
	public class VStack : AbstractLayout, IStackLayout
	{
		private readonly float? spacing;

		public VStack() : this (null)
		{
		}
		public VStack(float? spacing) 
		{
			this.spacing = spacing;
		}

		double IStackLayout.Spacing => (int)(spacing ?? 6);

		protected override ILayoutManager CreateLayoutManager() => new VerticalStackLayoutManager(this);
	}
}

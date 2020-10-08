
using System.Collections.Generic;
using Xamarin.Platform;
using Xamarin.Platform.Layouts;

namespace Comet
{
	public class HStack : AbstractLayout, IStackLayout
	{
		private readonly double? spacing;

		public HStack(
			VerticalAlignment alignment = VerticalAlignment.Center,
			double? spacing = null) : base()
		{
			this.spacing = spacing;
		}

		int IStackLayout.Spacing => (int)(spacing ?? 0);


		public override ILayoutManager CreateLayoutManager() => new HorizontalStackLayoutManager(this);
		ILayoutManager ILayout.CreateLayoutManager() => throw new System.NotImplementedException();
	}
}

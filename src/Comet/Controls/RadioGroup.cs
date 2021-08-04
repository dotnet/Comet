using Comet.Layout;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;

namespace Comet
{
	public class RadioGroup : AbstractLayout, IStackLayout
	{
		public RadioGroup(Orientation orientation = Orientation.Vertical)
			: base()
		{
			Orientation = orientation;
		}

		public Orientation Orientation { get; }

		double IStackLayout.Spacing => 6;

		protected override ILayoutManager CreateLayoutManager() => Orientation == Orientation.Vertical
				? new VerticalStackLayoutManager(this) : new HorizontalStackLayoutManager(this);

		protected override void OnAdded(View view)
		{
			if (view is RadioButton)
			{
				base.OnAdded(view);
			}
			else
			{
				throw new System.Exception("A RadioGroup may only contain RadioButtons");
			}
		}
	}
}

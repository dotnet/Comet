using Xamarin.Platform;
using Xamarin.Platform.Layouts;

namespace Comet
{
	public class RadioGroup : AbstractLayout, IStackLayout
	{
		public RadioGroup(Orientation orientation = Orientation.Vertical)
		{
			Orientation = orientation;
		}

		public Orientation Orientation { get; }

		int IStackLayout.Spacing => 0;

		public override ILayoutManager CreateLayoutManager() => Orientation == Orientation.Vertical
				  ? (ILayoutManager)new VerticalStackLayoutManager(this)
				  : new HorizontalStackLayoutManager(this);

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

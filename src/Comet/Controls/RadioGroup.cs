using Comet.Layout;

namespace Comet
{
	public class RadioGroup : AbstractLayout
	{
		public RadioGroup(Orientation orientation = Orientation.Vertical) 
			: base(orientation == Orientation.Vertical
				  ? (ILayoutManager)new VStackLayoutManager()
				  : (ILayoutManager)new HStackLayoutManager())
		{
			Orientation = orientation;
		}

		public Orientation Orientation { get; }

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

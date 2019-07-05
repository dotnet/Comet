using System;
namespace HotUI {
	public class NavigationView : ContentView {
		public void Navigate (View view)
		{
			view.Navigation = this;
			view.UpdateNavigation ();
			PerformNavigate (view);
		}
		public Action<View> PerformNavigate;

		public override void Add (View view)
		{
			base.Add (view);
			if (view != null) {
				view.Navigation = this;
				view.Parent = this;
			}
		}
	}
}

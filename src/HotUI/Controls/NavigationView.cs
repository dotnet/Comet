using System;
namespace HotUI {
	public class NavigationView : ContentView {
		public Action<View> Navigate;
		public static Action<(View FromView, View ToView)> NavigateModal;
		public override void Add (View view)
		{
			base.Add (view);
			if (view != null) {
				view.Parent = this;
			}
		}
	}
}

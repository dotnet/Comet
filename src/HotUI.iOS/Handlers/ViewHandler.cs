using System;
using UIKit;

namespace HotUI.iOS {
	public class ViewHandler : IUIView {
		public Action ViewChanged { get; set; }
		public ViewHandler ()
		{
		}

		public UIView View => currentView?.ToView () ?? new UIView();

		View currentView;
		public void Remove (View view)
		{
			currentView = null;
		}

		public void SetView (View view)
		{
			currentView = view;
			View.UpdateProperties (view);
			ViewChanged?.Invoke ();
		}

		public void UpdateValue (string property, object value)
		{
			View.UpdateBaseProperty (property, value);
		}
	}
}

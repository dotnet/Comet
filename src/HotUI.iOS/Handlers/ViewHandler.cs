using System;
using System.Diagnostics;
using UIKit;

namespace HotUI.iOS {
	public class ViewHandler : IUIView {
		public UIViewController CurrentViewController;
		public Action ViewChanged { get; set; }
		public ViewHandler ()
		{
		}

		public UIView View {
			get {
				var iView = currentView?.ToIUIView ();
				if (iView?.GetType () == typeof (ViewHandler) && currentView?.Body == null) {
					//This is recusrive!!!
					Debug.WriteLine ($"There is no View Handler for {currentView.GetType ()}");
					return new UIView ();
				}

				return iView?.View ?? new UIView ();
			}
		}

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

using System;
using AView = Android.Views.View;
namespace HotUI.Android {
	public class ViewHandler : IView {
		AView currentView;

		public AView View => currentView;
		public object NativeView => View;
		public bool HasContainer { get; set; } = false;

		public void Remove (View view)
		{
			// todo: implement this
		}

		public void SetView (View view)
		{
			currentView = view.ToView ();
			currentView?.UpdateProperties (view);
		}

		public void UpdateValue (string property, object value)
		{
			View?.UpdateProperty (property, value);
		}
	}
}

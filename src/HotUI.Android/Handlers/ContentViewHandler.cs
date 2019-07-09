using System;
using Android.Views;

namespace HotUI.Android {
	public class ContentViewHandler: IView {
		public ContentViewHandler ()
		{
		}

		public global::Android.Views.View View => _contentView?.Content?.ToView ();
		public object NativeView => View;
		public bool HasContainer { get; set; } = false;

		ContentView _contentView;

		public void Remove (View view)
		{
			_contentView = null;
		}

		public void SetView (View view)
		{
			_contentView = view as ContentView;
		}

		public void UpdateValue (string property, object value)
		{

		}
	}
}

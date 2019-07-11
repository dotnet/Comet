using System;
using Xamarin.Forms;

namespace HotUI.Forms {
	public class ContentViewHandler : FormsViewHandler {
		public ContentViewHandler ()
		{
		}

		public Xamarin.Forms.View View => _contentView?.Content?.ToForms ();
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

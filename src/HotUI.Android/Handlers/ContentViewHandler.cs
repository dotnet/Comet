using System;
using Android.Views;
using AView = global::Android.Views.View;

namespace HotUI.Android 
{
	public class ContentViewHandler: AndroidViewHandler
	{
		private AView _view;
		private ContentView _contentView;

		public ContentViewHandler ()
		{
		}

		public AView View => _view;
		
		public object NativeView => View;

		public bool HasContainer
		{
			get => false;
			set { }
		} 
		
		public void Remove (View view)
		{
			_view = null;
			_contentView = null;
		}

		public void SetView (View view)
		{
			_contentView = view as ContentView;
			_view = _contentView?.Content?.ToView ();
		}

		public void UpdateValue (string property, object value)
		{

		}
	}
}

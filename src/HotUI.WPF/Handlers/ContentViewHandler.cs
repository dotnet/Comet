using System;
using System.Windows;

namespace HotUI.WPF
{
	public class ContentViewHandler : IUIElement
    {
		public ContentViewHandler ()
		{
		}

		public UIElement View => ContentView?.Content.ToView ();

		public void Remove (View view)
		{
			ContentView = null;
		}

        private ContentView ContentView { get; set; }

		public void SetView (View view)
		{
			ContentView = view as ContentView;
		}

		public void UpdateValue (string property, object value)
		{
		}
	}
}

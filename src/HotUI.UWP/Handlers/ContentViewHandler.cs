using System;
using Windows.UI.Xaml;

namespace HotUI.UWP
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

		ContentView ContentView;
		public void SetView (View view)
		{
			ContentView = view as ContentView;
		}

		public void UpdateValue (string property, object value)
		{
		}
	}
}

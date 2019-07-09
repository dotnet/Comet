using System;
using Windows.UI.Xaml;

namespace HotUI.UWP
{
	public class ContentViewHandler : IUIElement
    {
        UIElement _view;

		public ContentViewHandler ()
		{
		}

		public UIElement View =>_view;

        public object NativeView => View;

        public bool HasContainer
        {
            get => false;
            set { }
        }

        public void Remove (View view)
		{
			ContentView = null;
		}

        ContentView ContentView { get; set; }

		public void SetView (View view)
		{
			ContentView = view as ContentView;
            _view = ContentView?.Content.ToView();
        }

		public void UpdateValue (string property, object value)
		{
		}
	}
}

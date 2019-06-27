using System;
using UIKit;

namespace HotUI.iOS {
	public class ContentViewHandler : IUIView {

		public UIView View => ContentView?.Content?.ToView();

		ContentView ContentView;
		public void Remove (View view)
		{
			ContentView = null;
		}

		public void SetView (View view)
		{
			ContentView = view as ContentView;
		}

		public void UpdateValue (string property, object value)
		{

		}
	}
}

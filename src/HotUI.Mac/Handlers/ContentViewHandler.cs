using System;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac {
	public class ContentViewHandler : INSView {
		public ContentViewHandler ()
		{
		}

		public NSView View => ContentView?.Content.ToView ();

		public object NativeView => View;
		public bool HasContainer { get; set; } = false;

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

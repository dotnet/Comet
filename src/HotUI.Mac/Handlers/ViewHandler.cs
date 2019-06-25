using System;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac {
	public class ViewHandler : INSView {
		public Action ViewChanged { get; set; }

		public NSView View => currentView?.ToView () ?? new NSView();

		View currentView;
		public void Remove (View view)
		{
			currentView = null;
		}

		public void SetView (View view)
		{
			currentView = view;
			View?.UpdateProperties (view);
			ViewChanged?.Invoke ();
		}

		public void UpdateValue (string property, object value)
		{
			View.UpdateBaseProperty (property, value);
		}
	}
}

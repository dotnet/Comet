using System;
using System.Diagnostics;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac {
	public class ViewHandler : INSView {
		public Action ViewChanged { get; set; }

		public NSView View {
			get {
				var iView = currentView?.ToINSView ();
				if(iView?.GetType() == typeof(ViewHandler) && currentView?.Body == null) {
					//This is recusrive!!!
					Debug.WriteLine ($"There is no View Handler for {currentView.GetType()}");
					return new NSView ();
				}

				return iView?.View ?? new NSView ();
			}
		}

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

using System;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers {
	public class EntryHandler : NSTextField, INSView  {
		public EntryHandler ()
		{
			EditingEnded += EntryHandler_Ended;
		}

		public NSView View => this;

		public void Remove (View view)
		{

		}
		Entry entry;
		public void SetView (View view)
		{
			entry = view as Entry;
			this.UpdateProperties (entry);

		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}

		void EntryHandler_Ended (object sender, EventArgs e) => entry?.Completed (this.StringValue);
	}


	public static partial class ControlExtensions {

		public static void UpdateProperties (this NSTextField view, Entry hView)
		{
			view.StringValue = hView?.Text;
			view.UpdateBaseProperties (hView);
		}

		public static bool UpdateProperty (this NSTextField view, string property, object value)
		{
			switch (property) {
			case nameof (Entry.Text):
				view.StringValue = (string)value;
				return true;
			}
			return view.UpdateBaseProperty (property, value);
		}
	}
}

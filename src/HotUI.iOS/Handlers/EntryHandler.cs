using System;
using UIKit;
namespace HotUI.iOS {
	public class EntryHandler : UITextField, IUIView  {
		public EntryHandler ()
		{
			this.Ended += EntryHandler_Ended;
		}

		public UIView View => this;

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

		void EntryHandler_Ended (object sender, EventArgs e) => entry?.Completed (this.Text);
	}


	public static partial class ControlExtensions {

		public static void UpdateProperties (this UITextField view, Entry hView)
		{
			view.Text = hView?.Text;
			view.UpdateBaseProperties (hView);
		}

		public static bool UpdateProperty (this UITextField view, string property, object value)
		{
			switch (property) {
			case nameof (Entry.Text):
				view.Text = (string)value;
				return true;
			}
			return view.UpdateBaseProperty (property, value);
		}
	}
}

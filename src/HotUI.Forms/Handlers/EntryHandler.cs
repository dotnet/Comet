using System;
using HotForms;
using Xamarin.Forms;
using FEntry = Xamarin.Forms.Entry;
using HEntry = HotForms.Entry;
using HView = HotForms.View;
namespace HotUI.Forms {
	public class EntryHandler : FEntry , HotForms.IViewHandler, IFormsView{
		public EntryHandler ()
		{
			this.Focused += FormsControl_Focused;
			this.TextChanged += FormsControl_TextChanged;
			this.Unfocused += FormsControl_Unfocused;
			this.Completed += FormsControl_Completed;
		}
		HEntry entry;
		public Xamarin.Forms.View View => this;

		public void Remove (HView view)
		{
			this.TextChanged -= FormsControl_TextChanged;
			this.Unfocused -= FormsControl_Unfocused;
			this.Completed -= FormsControl_Completed;
			this.Focused -= FormsControl_Focused;
		}

		public void SetView (HView view)
		{
			entry = view as HEntry;
			if (entry == null)
				return;

		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}


		private void FormsControl_Completed (object sender, EventArgs e) => entry?.Completed?.Invoke (Text);

		private void FormsControl_Unfocused (object sender, Xamarin.Forms.FocusEventArgs e) => entry?.Unfocused?.Invoke (entry);

		private void FormsControl_TextChanged (object sender, Xamarin.Forms.TextChangedEventArgs e) =>
			entry?.TextChanged?.Invoke ((e.NewTextValue, e.OldTextValue));

		private void FormsControl_Focused (object sender, Xamarin.Forms.FocusEventArgs e) => entry?.Focused?.Invoke (entry);

	}
}

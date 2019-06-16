using System;
using FControlType = Xamarin.Forms.Entry;
namespace HotForms {
	public class Entry : BaseControl<FControlType> {

		public Entry ()
		{
			FormsControl.Focused += FormsControl_Focused;
			FormsControl.TextChanged += FormsControl_TextChanged;
			FormsControl.Unfocused += FormsControl_Unfocused;
			FormsControl.Completed += FormsControl_Completed;
		}

		public string Text {
			get => FormsControl.Text;
			set => FormsControl.Text = value;
		}
		public string Placeholder {
			get => FormsControl.Placeholder;
			set => FormsControl.Placeholder = value;
		}

		public Action<string> Completed { get; set; }
		public Action<Entry> Focused { get; set; }
		public Action<Entry> Unfocused { get; set; }
		public Action<(string NewText,string OldText)> TextChanged { get; set; }


		private void FormsControl_Completed (object sender, EventArgs e) => Completed?.Invoke (Text);

		private void FormsControl_Unfocused (object sender, Xamarin.Forms.FocusEventArgs e) => Unfocused?.Invoke (this);

		private void FormsControl_TextChanged (object sender, Xamarin.Forms.TextChangedEventArgs e) =>
			TextChanged?.Invoke ((e.NewTextValue, e.OldTextValue));

		private void FormsControl_Focused (object sender, Xamarin.Forms.FocusEventArgs e) => Focused?.Invoke (this);
	}
}

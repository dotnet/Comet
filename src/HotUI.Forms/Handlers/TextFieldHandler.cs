using System;
using HotUI;
using Xamarin.Forms;
using FEntry = Xamarin.Forms.Entry;
using HView = HotUI.View;
namespace HotUI.Forms {
	public class TextFieldHandler : FEntry, IFormsView {
		public TextFieldHandler ()
		{
			this.Focused += FormsControl_Focused;
			this.TextChanged += FormsControl_TextChanged;
			this.Unfocused += FormsControl_Unfocused;
			this.Completed += FormsControl_Completed;
		}
		TextField _textField;
		public Xamarin.Forms.View View => this;
		public object NativeView => View;
		public bool HasContainer { get; set; } = false;

		public void Remove (HView view)
		{
			this.TextChanged -= FormsControl_TextChanged;
			this.Unfocused -= FormsControl_Unfocused;
			this.Completed -= FormsControl_Completed;
			this.Focused -= FormsControl_Focused;
		}

		public void SetView (HView view)
		{
			_textField = view as TextField;
			if (_textField == null)
				return;
			this.UpdateProperties (_textField);

		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}


		private void FormsControl_Completed (object sender, EventArgs e) => _textField?.Completed?.Invoke (Text);

		private void FormsControl_Unfocused (object sender, Xamarin.Forms.FocusEventArgs e) => _textField?.Unfocused?.Invoke (_textField);

		private void FormsControl_TextChanged (object sender, Xamarin.Forms.TextChangedEventArgs e) =>
			_textField?.TextChanged?.Invoke ((e.NewTextValue, e.OldTextValue));

		private void FormsControl_Focused (object sender, Xamarin.Forms.FocusEventArgs e) => _textField?.Focused?.Invoke (_textField);

	}
}

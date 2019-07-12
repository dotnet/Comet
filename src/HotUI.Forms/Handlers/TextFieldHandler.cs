using System;
using HotUI;
using Xamarin.Forms;
using FEntry = Xamarin.Forms.Entry;
using HView = HotUI.View;
namespace HotUI.Forms 
{
	public class TextFieldHandler : FEntry, FormsViewHandler 
	{
		TextField _textField;

		public TextFieldHandler ()
		{
			Focused += HandleFocused;
			TextChanged += HandleTextChanged;
			Unfocused += HandleUnfocused;
			Completed += HandleCompleted;
		}
		
		public Xamarin.Forms.View View => this;
		public object NativeView => View;
		public bool HasContainer { get; set; } = false;

		public void Remove (HView view)
		{
			
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
		
		private void HandleCompleted(object sender, EventArgs e)
		{
			_textField?.OnCommit?.Invoke(Text);
		}

		private void HandleUnfocused(object sender, FocusEventArgs e)
		{
			_textField?.Unfocused?.Invoke(_textField);
		}

		private void HandleTextChanged(object sender, TextChangedEventArgs e)
		{
			_textField?.OnEditingChanged?.Invoke(e.NewTextValue);
		}

		private void HandleFocused(object sender, FocusEventArgs e)
		{
			_textField?.Focused?.Invoke(_textField);
		}
	}
}

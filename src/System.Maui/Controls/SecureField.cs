using System;

namespace System.Maui
{
	public class SecureField : View
	{
		public SecureField(
			Binding<string> value = null,
			Binding<string> placeholder = null,
			Action<string> onCommit = null)
		{
			Text = value;
			Placeholder = placeholder;
			OnCommit = onCommit;
		}

		public SecureField(
			Func<string> value,
			Func<string> placeholder = null,
			Action<string> onCommit = null) : this((Binding<string>)value, (Binding<string>)placeholder, onCommit)
		{ }

		Binding<string> _text;
		public Binding<string> Text
		{
			get => _text;
			private set => this.SetBindingValue(ref _text, value);
		}


		Binding<string> _placeholder;
		public Binding<string> Placeholder
		{
			get => _placeholder;
			private set => this.SetBindingValue(ref _placeholder, value);
		}

		public Action<string> OnCommit { get; set; }
	}
}

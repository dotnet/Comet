using System;

namespace Comet
{
	public class TextField : View
	{
		public TextField(
			Binding<string> value = null,
			string placeholder = null,
			Action<string> onEditingChanged = null,
			Action<string> onCommit = null)
		{
			Text = value;
			Placeholder = placeholder;
			OnEditingChanged = new MulticastAction<string>(Text, onEditingChanged);
			OnCommit = onCommit;
		}
		public TextField(
			Func<string> value,
			string placeholder = null,
			Action<string> onEditingChanged = null,
			Action<string> onCommit = null) : this((Binding<string>)value, placeholder, onEditingChanged, onCommit)
		{

		}



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

		public Action<TextField> Focused { get; private set; }
		public Action<TextField> Unfocused { get; private set; }
		public Action<string> OnEditingChanged { get; private set; }
		public Action<string> OnCommit { get; private set; }

		public void ValueChanged(string value)
			=> OnEditingChanged?.Invoke(value);
	}
}

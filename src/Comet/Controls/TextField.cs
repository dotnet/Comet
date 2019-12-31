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
			Text = value ?? new Binding<string>(
				() => this.value ??= "",
				(outVal) => this.value = outVal
				);
			Text.Set ??= (v) => {
				Text.BindingValueChanged(null, nameof(Text), v);
				this.ViewPropertyChanged(nameof(Text), v);
			};
			Placeholder = placeholder;
			OnEditingChanged = new MulticastAction<string>(value, onEditingChanged);
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

		string value;
		public void ValueChanged(string value)
		{
			OnEditingChanged?.Invoke(value);

			//If the value stored is null, there is no real binding.
			//SO we need to fire the notification ourselves..
			if (this.value != null)
			{
				Text.BindingValueChanged(null, nameof(Text), value);
				this.ViewPropertyChanged(nameof(Text), value);
			}
		}
	}
}

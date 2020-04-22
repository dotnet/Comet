using System;

namespace System.Maui
{
	/// <summary>
	/// A view that displays one or more lines of read-only text.
	/// </summary>
	public class Label : View
	{
		public Label(
			Binding<string> value = null)
		{
			Value = value;
		}
		public Label(
			Func<string> value) : this((Binding<string>)value)
		{

		}

		Binding<string> _value;
		public Binding<string> Value
		{
			get => _value;
			private set => this.SetBindingValue(ref _value, value);
		}

		public override string ToString() => _value?.Value?.ToString() ?? string.Empty;
	}
}

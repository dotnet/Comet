using System;
using Microsoft.Maui;
namespace Comet
{
	public class CheckBox : View, ICheckBox
	{
		public CheckBox(
			Binding<bool> value = null,
			Action<bool> onChanged = null)
		{
			IsOn = value;
			IsOnChanged = new MulticastAction<bool>(IsOn, onChanged);
		}

		Binding<bool> _isOn;
		public Binding<bool> IsOn
		{
			get => _isOn;
			private set => this.SetBindingValue(ref _isOn, value);
		}

		public Action<bool> IsOnChanged { get; private set; }
		bool ICheckBox.IsChecked { get => IsOn; set => IsOn.Set(value); }
	}
}

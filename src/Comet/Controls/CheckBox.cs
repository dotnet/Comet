using System;
using System.Collections.Generic;
using Microsoft.Maui;
namespace Comet
{
	public class CheckBox : View, ICheckBox
	{
		protected static Dictionary<string, string> CheckBoxHandlerPropertyMapper = new(HandlerPropertyMapper)
		{
			[nameof(IsOn)] = nameof(ICheckBox.IsChecked),
		};
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
		protected override string GetHandlerPropertyName(string property)
			=> CheckBoxHandlerPropertyMapper.TryGetValue(property, out var value) ? value : property;
	}
}

using System;

namespace Comet
{
	public class ProgressBar : View
	{
		public ProgressBar(
			Binding<float> value = null,
			bool isIndeterminate = false)
		{
			Value = value;
			IsIndeterminate = isIndeterminate;
		}

		public ProgressBar(
			Func<float> value,
			bool isIndeterminate = false) : this((Binding<float>)value, isIndeterminate)
		{ }

		Binding<bool> _isIndeterminate;
		public Binding<bool> IsIndeterminate
		{
			get => _isIndeterminate;
			set => this.SetBindingValue(ref _isIndeterminate, value);
		}

		Binding<float> _value;
		public Binding<float> Value
		{
			get => _value;
			set => this.SetBindingValue(ref _value, value);
		}
	}
}

using System;

namespace Comet
{
	public class ProgressBar : View
	{
		public ProgressBar(
			Binding<double> value = null,
			bool isIndeterminate = false)
		{
			Value = value;
			IsIndeterminate = isIndeterminate;
		}

		public ProgressBar(
			Func<double> value,
			bool isIndeterminate = false) : this((Binding<double>)value, isIndeterminate)
		{ }

		Binding<bool> _isIndeterminate;
		public Binding<bool> IsIndeterminate
		{
			get => _isIndeterminate;
			set => this.SetBindingValue(ref _isIndeterminate, value);
		}

		Binding<double> _value;
		public Binding<double> Value
		{
			get => _value;
			set => this.SetBindingValue(ref _value, value);
		}
	}
}

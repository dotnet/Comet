using System;

namespace Comet
{
	public class ProgressBar : View
	{
		public ProgressBar(
			Binding<double> value = null)
		{
			Value = value;
		}

		public ProgressBar(
			Func<double> value) : this((Binding<double>)value)
		{ }

		Binding<double> _value;
		public Binding<double> Value
		{
			get => _value;
			set => this.SetBindingValue(ref _value, value);
		}
	}
}

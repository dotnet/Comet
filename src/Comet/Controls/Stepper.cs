using System;
using System.Collections.Generic;
using System.Text;

namespace Comet
{
	public class Stepper : View
	{
		public Stepper(Binding<double> value = null,
			Binding<double> maximumValue = null,
			Binding<double> minimumValue = null,
			Binding<double> increment = null,
			Action<double> onValueChanged = null)
		{
			if (minimumValue >= maximumValue)
				throw new ArgumentOutOfRangeException(nameof(minimumValue), "Minimum value is greater than the maximum value");
			Value = value;
			Maximum = maximumValue;
			Minimum = minimumValue;
			Increment = increment;
			OnValueChanged = new MulticastAction<double>(Value, onValueChanged);
		}

		Binding<double> _value;
		public Binding<double> Value
		{
			get => _value;
			private set => this.SetBindingValue(ref _value, value);
		}

		Binding<double> _maximum;
		public Binding<double> Maximum
		{
			get => _maximum;
			private set => this.SetBindingValue(ref _maximum, value);
		}

		Binding<double> _minimum;
		public Binding<double> Minimum
		{
			get => _minimum;
			private set => this.SetBindingValue(ref _minimum, value);
		}

		Binding<double> _increment;
		public Binding<double> Increment
		{
			get => _increment;
			private set => this.SetBindingValue(ref _increment, value);
		}

		public Action<double> OnValueChanged { get; private set; }
	}
}

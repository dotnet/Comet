using System;

namespace Comet
{
	public class ProgressBar : View
	{
		public ProgressBar(
			Binding<float> value = null)
		{
			Value = value;
		}

		public ProgressBar(
			Func<float> value) : this((Binding<float>)value)
		{ }

		Binding<float> _value;
		public Binding<float> Value
		{
			get => _value;
			set => this.SetBindingValue(ref _value, value);
		}
	}
}

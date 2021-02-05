using System;
using Comet.Internal;
using Xamarin.Platform;

namespace Comet
{
	public class Slider : View, ISlider
	{
		public Slider(
			Binding<double> value = null,
			double from = 0,
			double through = 100,
			double by = 1,
			Action<double> onEditingChanged = null)
		{
			Value = value;
			From = from;
			Through = through;
			By = by;
			OnEditingChanged = new MulticastAction<double>(Value, onEditingChanged);
		}

		Binding<double> _value;
		public Binding<double> Value
		{
			get => _value;
			private set => this.SetBindingValue(ref _value, value);
		}

		Binding<double> _from;
		public Binding<double> From
		{
			get => _from;
			private set => this.SetBindingValue(ref _from, value);
		}

		Binding<double> _through;
		public Binding<double> Through
		{
			get => _through;
			private set => this.SetBindingValue(ref _through, value);
		}

		Binding<double> _by;
		public Binding<double> By
		{
			get => _by;
			private set => this.SetBindingValue(ref _by, value);
		}

		public Action<double> OnEditingChanged { get; private set; }

		double ISlider.Minimum => From;

		double ISlider.Maximum => Through;

		double ISlider.Value { get => Value; set => Value.Set(value); }

		System.Graphics.Color ISlider.MinimumTrackColor => this.GetTrackColor();

		System.Graphics.Color ISlider.MaximumTrackColor => this.GetProgressColor();

		System.Graphics.Color ISlider.ThumbColor => this.GetThumbColor();

		public void ValueChanged(double value)
			=> OnEditingChanged.Invoke(value);

		public void PercentChanged(double percent)
		{
			var from = From.CurrentValue;
			var value = ((Through.CurrentValue - from) * percent.Clamp(0, 1)) + from;
			ValueChanged(value);
		}

		void ISlider.DragStarted() {}
		void ISlider.DragCompleted() { }
	}
}

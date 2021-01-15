using System;
using Comet.Internal;
using Xamarin.Platform;

namespace Comet
{
	public class Slider : View, ISlider
	{
		public Slider(
			Binding<float> value = null,
			float from = 0,
			float through = 100,
			float by = 1,
			Action<float> onEditingChanged = null)
		{
			Value = value;
			From = from;
			Through = through;
			By = by;
			OnEditingChanged = new MulticastAction<float>(Value, onEditingChanged);
		}

		Binding<float> _value;
		public Binding<float> Value
		{
			get => _value;
			private set => this.SetBindingValue(ref _value, value);
		}

		Binding<float> _from;
		public Binding<float> From
		{
			get => _from;
			private set => this.SetBindingValue(ref _from, value);
		}

		Binding<float> _through;
		public Binding<float> Through
		{
			get => _through;
			private set => this.SetBindingValue(ref _through, value);
		}

		Binding<float> _by;
		public Binding<float> By
		{
			get => _by;
			private set => this.SetBindingValue(ref _by, value);
		}

		public Action<float> OnEditingChanged { get; private set; }

		float ISlider.Minimum => From;

		float ISlider.Maximum => Through;

		float ISlider.Value { get => Value; set => Value.Set(value); }

		System.Graphics.Color ISlider.MinimumTrackColor => this.GetTrackColor();

		System.Graphics.Color ISlider.MaximumTrackColor => this.GetProgressColor();

		System.Graphics.Color ISlider.ThumbColor => this.GetThumbColor();

		public void ValueChanged(float value)
			=> OnEditingChanged.Invoke(value);

		public void PercentChanged(float percent)
		{
			var from = From.CurrentValue;
			var value = ((Through.CurrentValue - from) * percent.Clamp(0, 1)) + from;
			ValueChanged(value);
		}

		void ISlider.DragStarted() {}
		void ISlider.DragCompleted() { }
	}
}

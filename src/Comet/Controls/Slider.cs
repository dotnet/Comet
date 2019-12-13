using System;
using Comet.Internal;

namespace Comet
{
	public class Slider : View
	{
		public Slider(
			Binding<float> value = null,
			float from = 0,
			float through = 100,
			float by = 1,
			Action<float> onEditingChanged = null)
		{
			Value = value ?? new Binding<float>(
				() => this.value ?? 50f,
				(outVal) => this.value = outVal
				);
			From = from;
			Through = through;
			By = by;
			OnEditingChanged = new MulticastAction<float>(Value, onEditingChanged);
		}
		float? value;
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

		public void ValueChanged(float value)
		{
			OnEditingChanged?.Invoke(value);

			//If the value stored is null, there is no real binding.
			//SO we need to fire the notification ourselves..
			if (this.value != null){
				Value.BindingValueChanged(null, nameof(Value), value);
				this.ViewPropertyChanged(nameof(Value), value);
			}

			//Value.Set(value);
		}
		public void PercentChanged(float percent)
		{
			var from = From.CurrentValue;
			var value = ((Through.CurrentValue - from) * percent.Clamp(0,1)) + from;
			ValueChanged(value);
		}
	}
}

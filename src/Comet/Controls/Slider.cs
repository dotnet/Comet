using System;
using System.Collections.Generic;
using Comet.Internal;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class Slider : View, ISlider, IThumbView
	{
		protected static Dictionary<string, string> SliderHandlerPropertyMapper = new()
		{
			[nameof(EnvironmentKeys.Slider.TrackColor)] = nameof(ISlider.MinimumTrackColor),
			[nameof(EnvironmentKeys.Slider.ProgressColor)] = nameof(ISlider.MaximumTrackColor),
			[nameof(From)] = nameof(IRange.Minimum),
			[nameof(Through)] = nameof(IRange.Maximum),
		};

		public Slider(
			Binding<double> value = null,
			double from = 0,
			double through = 1,
			double by = .1,
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

		Color ISlider.MinimumTrackColor => this.GetTrackColor();

		Color ISlider.MaximumTrackColor => this.GetProgressColor();

		Color ISlider.ThumbColor => this.GetThumbColor();

		double IRange.Minimum => From;

		double IRange.Maximum => Through;

		double IRange.Value {
			get => Value;
			set => ValueChanged(value);
		}

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

		protected override string GetHandlerPropertyName(string property) =>
			SliderHandlerPropertyMapper.TryGetValue(property, out var value) ? value : base.GetHandlerPropertyName(property);
	}
}

using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class TimePicker : View, ITimePicker
	{
		public TimePicker(Binding<TimeSpan> time = null,
			Binding<string> format = null,
			Action<TimeSpan> onTimeChanged = null)
		{
			Value = time;
			Format = format;
			OnTimeChanged = new MulticastAction<TimeSpan>(Value, onTimeChanged);
		}


		Binding<TimeSpan> _maximumDate;
		public Binding<TimeSpan> MaximumDate
		{
			get => _maximumDate;
			private set => this.SetBindingValue(ref _maximumDate, value);
		}

		Binding<TimeSpan> _minimumDate;
		public Binding<TimeSpan> MinimumDate
		{
			get => _minimumDate;
			private set => this.SetBindingValue(ref _minimumDate, value);
		}

		Binding<string> _format;
		public Binding<string> Format
		{
			get => _format;
			private set => this.SetBindingValue(ref _format, value);
		}

		Binding<TimeSpan> _value;
		public Binding<TimeSpan> Value
		{
			get => _value;
			private set => this.SetBindingValue(ref _value, value);
		}

		public Action<TimeSpan> OnTimeChanged { get; private set; }

		string ITimePicker.Format => Format;

		TimeSpan ITimePicker.Time {
			get => Value;
			set => OnTimeChanged?.Invoke(value);
		 }

		double ITextStyle.CharacterSpacing => this.GetEnvironment<double>(nameof(IDatePicker.CharacterSpacing));

		Font ITextStyle.Font => this.GetFont(null);

		Color ITextStyle.TextColor => this.GetColor();
	}
}

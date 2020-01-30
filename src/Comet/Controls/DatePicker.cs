using System;
namespace Comet
{
	public class DatePicker : View
	{
		public DatePicker(Binding<DateTime> date = null,
			Binding < DateTime> maximumDate = null,
			Binding<DateTime> minimumDate = null,
			Binding<string> format = null,
			Action<DateTime> onDateChnaged = null)
		{
			if (minimumDate?.CurrentValue >= maximumDate?.CurrentValue)
				throw new ArgumentOutOfRangeException(nameof(minimumDate), "Minimum date is greater than the maximum date");
			Date = date;
			MaximumDate = maximumDate;
			MinimumDate = minimumDate;
			Format = format;
			OnDateChanged = new MulticastAction<DateTime>(date, onDateChnaged);
		}


		Binding<DateTime> _maximumDate;
		public Binding<DateTime> MaximumDate
		{
			get => _maximumDate;
			private set => this.SetBindingValue(ref _maximumDate, value);
		}

		Binding<DateTime> _minimumDate;
		public Binding<DateTime> MinimumDate
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

		Binding<DateTime> _date;
		public Binding<DateTime> Date
		{
			get => _date;
			private set => this.SetBindingValue(ref _date, value);
		}

		public Action<DateTime> OnDateChanged { get; private set; }
	}
}

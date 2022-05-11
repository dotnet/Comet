using System;
namespace Comet.Samples
{
	public class DatePickerSample : View
	{
		readonly State<DateTime> currentDate = DateTime.Today;
		public DatePickerSample()
		{
			currentDate.PropertyChanged += CurrentDate_PropertyChanged;
		}

		[Body]
		View body() => new VStack
		{
			new DatePicker(currentDate, minimumDate: new DateTime(2015, 10, 1),
				maximumDate: new DateTime(2018, 10, 01)).Format("dd/MM/yyyy")
			.Frame(width: 200)
		};



		private void CurrentDate_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Console.WriteLine((sender as State<DateTime>)?.Value);
		}
	}
}

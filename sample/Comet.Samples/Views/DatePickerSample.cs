using System;
namespace Comet.Samples
{
	public class DatePickerSample : View
	{
		[Body]
		View body() => new VStack
		{
			new DatePicker(minimumDate: new DateTime(2015, 10, 1),
				maximumDate: new DateTime(2018, 10, 01),
				format: "dd/MM/yyyy", onDateChnaged: SampleDateChanged)
			.Frame(width: 200)
		};

		private void SampleDateChanged(DateTime value)
		{
			Console.WriteLine(value);
		}
	}
}

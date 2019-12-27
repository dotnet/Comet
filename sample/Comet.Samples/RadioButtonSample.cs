namespace Comet.Samples
{
	public class RadioButtonSample : View
	{
		[Body]
		View Body() => new VStack
		{
			new RadioGroup
			{
				new RadioButton(
					label: "Group 1: Option A",
					selected: true,
					onClick: () => System.Diagnostics.Debug.WriteLine("Option A selected")),
				new RadioButton(
					label: "Group 1: Option B",
					onClick: () => System.Diagnostics.Debug.WriteLine("Option B selected")),
				new RadioButton(
					label: "Group 1: Option C",
					onClick: () => System.Diagnostics.Debug.WriteLine("Option C selected")),
			},
			new RadioGroup(
				orientation: Orientation.Horizontal)
			{
				new RadioButton(
					label: "Implicit Group: Option 1",
					onClick: () => System.Diagnostics.Debug.WriteLine("Option 1 selected")),
				new RadioButton(
					label: "Implicit Group: Option 2",
					selected: true,
					onClick: () => System.Diagnostics.Debug.WriteLine("Option 2 selected")),
				new RadioButton(
					label: "Implicit Group: Option 3",
					onClick: () => System.Diagnostics.Debug.WriteLine("Option 3 selected"))
			}
		};
	}
}

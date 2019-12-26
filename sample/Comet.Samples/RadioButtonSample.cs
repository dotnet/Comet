namespace Comet.Samples
{
	public class RadioButtonSample : View
	{
		[Body]
		View Body() => new VStack
		{
			new RadioButton(
				label: "Group 1: Option A", 
				selected: true, 
				groupName: "group1", 
				onClick: () => System.Diagnostics.Debug.WriteLine("Option A selected")),
			new RadioButton(
				label: "Group 1: Option B", 
				groupName: "group1", 
				onClick: () => System.Diagnostics.Debug.WriteLine("Option B selected")),
			new RadioButton(
				label: "Group 1: Option C", 
				groupName: "group1", 
				onClick: () => System.Diagnostics.Debug.WriteLine("Option C selected")),

			new VStack
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

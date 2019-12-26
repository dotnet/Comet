namespace Comet.Samples
{
	public class RadioButtonSample : View
	{
		// TODO: Add state to register to Text to display OnClick responses

		[Body]
		View Body() => new VStack
		{
			new RadioButton("Group 1: Option A", true, "group1"),
			new RadioButton("Group 1: Option B", false, "group1"),
			new RadioButton("Group 1: Option C", false, "group1"),

			new VStack
			{
				new RadioButton("Implicit Group: Option A", true),
				new RadioButton("Implicit Group: Option B"),
				new RadioButton("Implicit Group: Option C")
			}
		};
	}
}

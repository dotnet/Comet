using System;
namespace System.Maui.Samples
{
	public class InsaneDiffPage : View
	{
		readonly State<bool> myBoolean = new State<bool>();
		readonly State<string> myText = new State<string>();

		[Body]
		View body()
		{
			var stack = new VStack {
					new Button
						(()=> myBoolean.Value ? myText.Value : $"State: {myBoolean.Value}",
						()=> myBoolean.Value = !myBoolean.Value),
				};
			for (var i = 0; i < 100; i++)
			{
				stack.Add(new Label (i.ToString()));
			}
			return new ScrollView { stack };
		}
	}
}

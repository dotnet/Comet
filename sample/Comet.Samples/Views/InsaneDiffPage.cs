using System;
namespace Comet.Samples
{
	public class InsaneDiffPage : View
	{
		readonly State<bool> myBoolean = new State<bool>();
		readonly State<string> myText = "";

		[Body]
		View body()
		{
			var stack = new VStack {
					//new Button
					//	(()=> myBoolean.Value ? myText.Value : $"State: {myBoolean.Value}",
					//	()=> myBoolean.Value = !myBoolean.Value),
					new Text(()=> $"{myText} - Test" )
			};
			for (var i = 0; i < 1000; i++)
			{
				stack.Add(new Text($"{myText} - {i}"));
			}
			return new ScrollView { stack };
		}
	}
}

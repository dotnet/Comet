using System;
using System.Collections.Generic;

namespace System.Maui.Samples
{
	public class ButtonSample1 : View
	{
		readonly State<int> count = 0;

		[Body]
		View body() => new VStack
		{
			new Button("Increment Value", () => count.Value ++ ),
			new Label(() => $"Value: {count.Value}"),
		};

	}
}

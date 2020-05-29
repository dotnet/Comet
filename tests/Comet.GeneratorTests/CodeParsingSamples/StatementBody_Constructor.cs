using Comet;

using System;
using System.Reflection;

public class MyView : View
{ 
	State<int> count = 0; 
	State<bool> shouldCelebrate = false;

	public MyView()
	{
		Body = () => new VStack
		{
			(shouldCelebrate ?(View)new Text($"{count.Value}") : new Spacer()),
			new Text(()=>$"{count} times"),
			new Button("Click Me!", ()=> count.Value++)
		};
	}

	public MyView(int blah)
	{
		Func<View> Body = () => new Text(() => $"{count} times");

		Body = () => new Text(() => $"{count} times");
	}
}
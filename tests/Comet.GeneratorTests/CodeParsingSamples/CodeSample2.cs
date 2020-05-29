using Comet;

public class MyView : View
{ 
	State<int> count = 0; 
	State<bool> shouldCelebrate = false;

	[Body]
	View body()
	{
		return new VStack
		{
			(shouldCelebrate ?(View)new Text($"{count.Value}") : new Spacer()),
			new Text(()=>$"{count} times"),
			new Button("Click Me!", ()=> count.Value++),
		};
	}
}
using Comet;

public class MyView : View
{ 
	State<int> count = 0; 
	State<bool> shouldCelebrate = false; 

	[Environment]
	public readonly string Foo;

	//[Body]
	//View body() => new Text(()=>$"{count} times");

	//[Body]
	//View body() => new VStack
	//{
	//	//(shouldCelebrate ?(View)new Text("Celebrate!!!!") : new Spacer()),
	//	new Text(()=>$"{count} times"),
	//	new Button("Click Me!", ()=> count.Value++),
	//};

	[Body]
	View body()
	{
		return new VStack
		{
			//(shouldCelebrate ?(View)new Text("Celebrate!!!!") : new Spacer()),
			new Text(()=>$"{count} times"),
			new Button("Click Me!", ()=> count.Value++),
		};
	}
}
using System;
namespace System.Maui.Samples
{
	public class TabViewSample : View
	{

		[Body]
		View body() => new TabView
		{
			new HStack{
				new Label("Tab 1")
			}.TabText("Tab 1"),
			new HStack
			{
				new Label("Tab 2"),
			}.TabText("Tab 2")
		};
	}
}

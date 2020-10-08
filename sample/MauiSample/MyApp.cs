using Comet;
using Xamarin.Forms;
using Xamarin.Platform;
using Xamarin.Platform.Core;
using Xamarin.Platform.Handlers;

namespace MauiSample
{
	public class MyApp : IApp
	{
		public MyApp()
		{
			CometPlatform.Init();
			var v = new View
			{
				Body = () => new Text("Hey"),
			};
			
		}

		public IView CreateView() => new VStack(spacing:5)
		{
			new Text("This top part is a Xamarin.Platform.VerticalStackLayout"),
			new HStack(spacing:2)
			{
				new Button("A Button").Frame(width:100).Color(Comet.Color.White),
				new Button("Hello I'm a button")
					.Color(Comet.Color.Green)
					.Background(Comet.Color.Purple),
				new Text("And these buttons are in a HorizontalStackLayout"),
			}
		}.Background(Comet.Color.Beige);
		
	}

	//public class Spacer : VerticalStackLayout
	//{
	//	//public override Size Measure(double widthConstraint, double heightConstraint)
	//	//{
	//	//	return base.Measure(widthConstraint, heightConstraint);
	//	//}
	//}
}
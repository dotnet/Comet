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
#if DEBUG
			Comet.Reload.Init();
#endif
			var v = new View
			{
				Body = () => new Text("Hey"),
			};
			
		}

		public IView CreateView() => new MainPage();
		
	}

	//public class Spacer : VerticalStackLayout
	//{
	//	//public override Size Measure(double widthConstraint, double heightConstraint)
	//	//{
	//	//	return base.Measure(widthConstraint, heightConstraint);
	//	//}
	//}
}
using System;
using Foundation;
using UIKit;
using Xamarin.Platform;
using Xamarin.Platform.Handlers;

namespace MauiSample.iOS {
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate, IUIApplicationDelegate {


		UIWindow _window;

		public override UIWindow Window
		{
			get;
			set;
		}

		static Random Random = new Random();
		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			//ViewHandler.ViewMapper["Border"] = (renderer, v) => {
			//	var r = Random.Next(0, 255);
			//	var g = Random.Next(0, 255);
			//	var b = Random.Next(0, 255);
			//	var borderColor = UIColor.FromRGB(r, g, b);
			//	var view = renderer.NativeView as UIView;
			//	view.Layer.BorderColor = borderColor.CGColor;
			//	view.Layer.BorderWidth = 2;
			//};

			ViewHandler.ViewMapper.Actions[nameof(IView.Frame)] = (r, view) => {
				Console.WriteLine($"Setting Frame: {view.Frame}  -  {view}");
				ViewHandler.MapPropertyFrame(r, view);
			};
			_window = new UIWindow();
			var app = new MyApp();

			IView content = app.CreateView();

			_window.RootViewController = new UIViewController
			{
				View = content.ToNative()
			};

			_window.MakeKeyAndVisible();

			return true;
		}

	}
}


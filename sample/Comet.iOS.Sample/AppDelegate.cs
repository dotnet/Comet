using System.Collections.Generic;
using CoreLocation;
using Foundation;
using Comet.Samples;
using MapKit;
using UIKit;
using Microsoft.Maui;
using Microsoft.Maui.HotReload;

namespace Comet.iOS.Sample
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{

		UIWindow _window;
		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{

			CometPlatform.Init();
			_window = new UIWindow();
			var app = new SampleApp();

			IView content = app;

			_window.RootViewController = new RootViewController
			{
				ContentView = content,
			};

			_window.MakeKeyAndVisible();
			//Adds the material Style
			//new MaterialStyle(ColorPalette.Blue).Apply();

			//Enables Skia
			//Comet.Skia.UI.Init();

			//Replaces Native controls with Skia Controls
			//Comet.Skia.Controls.Init();

			//Replaces the native controls with controls from Googles Material Lib
			//Comet.Material.iOS.UI.Init();
			return true;
			//return base.FinishedLaunching(application, launchOptions);
		}

		//protected override CometApp CreateApp() => new SampleApp();
	}

	
}


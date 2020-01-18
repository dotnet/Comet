using System.Collections.Generic;
using CoreLocation;
using Foundation;
using Comet.Samples;
using MapKit;
using UIKit;
using Comet.Styles;
using Comet.Styles.Material;

namespace Comet.iOS.Sample
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		public override UIWindow Window
		{
			get;
			set;
		}

		UIWindow window;

		public override void FinishedLaunching(UIApplication application)
		{
			base.FinishedLaunching(application);
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
#if DEBUG
			Comet.Reload.Init();
#endif
			UI.Init();
			//Adds the material Style
			//new MaterialStyle(ColorPalette.Blue).Apply();

			//Enables Skia
			Comet.Skia.UI.Init();

			//Replaces Native controls with Skia Controls
			//Comet.Skia.Controls.Init();

			//Replaces the native controls with controls from Googles Material Lib
			//Comet.Material.iOS.UI.Init();

			"turtlerock.jpg".LoadImage();
			window = new UIWindow
			{
				RootViewController = new MainPage(new List<MenuItem>
				{
					new MenuItem("SwiftUI Tutorial Section 5 (Native)", () => new Section5Native()),
					new MenuItem("SwiftUI Tutorial Section 5b (Native)", () => new Section5NativeB())
				}).ToViewController(),
				//RootViewController = new NestedViews().ToViewController(),
			};
			/*window = new UIWindow
            {
                RootViewController = new Issue123().ToViewController(),
            };*/
			window.MakeKeyAndVisible();

			return true;
		}


		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message)
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background execution this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transition from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive.
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}

	public class Section5Native : View
	{
		public Section5Native()
		{
			Body = () => new VStack
			{
				new ViewRepresentable()
				{
					MakeView = () => new MKMapView(UIScreen.MainScreen.Bounds),
					UpdateView = (view, data) =>
					{
						var mapView = (MKMapView)view;
						var coordinate = new CLLocationCoordinate2D(latitude: 34.011286, longitude: -116.166868);
						var span = new MKCoordinateSpan(latitudeDelta: 2.0, longitudeDelta: 2.0);
						var region = new MKCoordinateRegion(center: coordinate, span: span);
						mapView.SetRegion(region, animated: true);
					}
				}
			};
		}
	}

	public class Section5NativeB : View
	{
		public Section5NativeB()
		{
			Body = () => new VStack
			{
				new UIViewRepresentable<MKMapView>()
				{
					MakeView = () => new MKMapView(UIScreen.MainScreen.Bounds),
					UpdateView = (view, data) =>
					{
						var coordinate = new CLLocationCoordinate2D(latitude: 34.011286, longitude: -116.166868);
						var span = new MKCoordinateSpan(latitudeDelta: 2.0, longitudeDelta: 2.0);
						var region = new MKCoordinateRegion(center: coordinate, span: span);
						view.SetRegion(region, animated: true);
					}
				}
			};
		}
	}
}


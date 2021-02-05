using System.Collections.Generic;
using CoreLocation;
using Foundation;
using Comet.Samples;
using MapKit;
using UIKit;
using Comet.Styles;
using Comet.Styles.Material;
using Xamarin.Platform;
using Xamarin.Platform.HotReload;

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
#if DEBUG
			Reloadify.Reload.Instance.ReplaceType = (d) => HotReloadHelper.RegisterReplacedView(d.ClassName, d.Type);
			Reloadify.Reload.Instance.FinishedReload = () => HotReloadHelper.TriggerReload();
			Reloadify.Reload.Init();
			//Comet.Reload.Init();
#endif

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

	public class RootViewController : UIViewController
	{
		public IView ContentView { get; set; }
		UIView _contentView;
		public override void LoadView()
		{
			base.LoadView();
			View.AddSubview(_contentView = ContentView?.ToNative());
		}
		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();
			_contentView.Frame = View.Bounds;
		}
	}

	//public class Section5Native : View
	//{
	//	public Section5Native()
	//	{
	//		Body = () => new VStack
	//		{
	//			new ViewRepresentable()
	//			{
	//				MakeView = () => new MKMapView(UIScreen.MainScreen.Bounds),
	//				UpdateView = (view, data) =>
	//				{
	//					var mapView = (MKMapView)view;
	//					var coordinate = new CLLocationCoordinate2D(latitude: 34.011286, longitude: -116.166868);
	//					var span = new MKCoordinateSpan(latitudeDelta: 2.0, longitudeDelta: 2.0);
	//					var region = new MKCoordinateRegion(center: coordinate, span: span);
	//					mapView.SetRegion(region, animated: true);
	//				}
	//			}
	//		};
	//	}
	//}

	//public class Section5NativeB : View
	//{
	//	public Section5NativeB()
	//	{
	//		Body = () => new VStack
	//		{
	//			new UIViewRepresentable<MKMapView>()
	//			{
	//				MakeView = () => new MKMapView(UIScreen.MainScreen.Bounds),
	//				UpdateView = (view, data) =>
	//				{
	//					var coordinate = new CLLocationCoordinate2D(latitude: 34.011286, longitude: -116.166868);
	//					var span = new MKCoordinateSpan(latitudeDelta: 2.0, longitudeDelta: 2.0);
	//					var region = new MKCoordinateRegion(center: coordinate, span: span);
	//					view.SetRegion(region, animated: true);
	//				}
	//			}
	//		};
	//	}
	//}
}


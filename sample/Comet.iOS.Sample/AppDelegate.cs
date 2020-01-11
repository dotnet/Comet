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
	public class AppDelegate : CometAppDelegate
	{
		
		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
#if DEBUG
			Comet.Reload.Init();
#endif
			//Adds the material Style
			//new MaterialStyle(ColorPalette.Blue).Apply();

			//Enables Skia
			Comet.Skia.UI.Init();

			//Replaces Native controls with Skia Controls
			//Comet.Skia.Controls.Init();

			//Replaces the native controls with controls from Googles Material Lib
			//Comet.Material.iOS.UI.Init();

			return base.FinishedLaunching(application, launchOptions);
		}

		protected override CometApp CreateApp() => new SampleApp();
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


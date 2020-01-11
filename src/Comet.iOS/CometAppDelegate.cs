using System;
using Foundation;
using UIKit;
using Comet.Internal;
namespace Comet.iOS
{
	public abstract class CometAppDelegate : UIApplicationDelegate
	{
		protected abstract CometApp CreateApp();
		UIWindow window;
		public CometApp App { get; private set; }
		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			UI.Init();
			var app = CreateApp();
			(app as IApplicationLifeCycle)?.OnInit();
			window = new UIWindow
			{
				RootViewController = app.ToViewController(),
			};			
			window.MakeKeyAndVisible();
			return true;
		}

		public override void WillEnterForeground(UIApplication application)
			=> (App as IApplicationLifeCycle).OnResume();

		public override void OnResignActivation(UIApplication application)
			=> (App as IApplicationLifeCycle)?.OnPause();

		public override void WillTerminate(UIApplication application)
			=> (App as IApplicationLifeCycle).Dispose();
	}
}

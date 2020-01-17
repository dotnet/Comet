using System;
using Foundation;
using UIKit;
using Comet.Internal;
namespace Comet.iOS
{
	public abstract class CometAppDelegate : UIApplicationDelegate, IReloadHandler
	{
		protected abstract CometApp CreateApp();
		UIWindow window;
		public CometApp App { get; private set; }
		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			UI.Init();
			App = CreateApp();
			(App as IApplicationLifeCycle)?.OnInit();
			window = new UIWindow
			{
				RootViewController = App.ToViewController(),
			};			
			window.MakeKeyAndVisible();
			App.ReloadHandler = this;
			return true;
		}

		public override void WillEnterForeground(UIApplication application)
			=> (App as IApplicationLifeCycle).OnResume();

		public override void OnResignActivation(UIApplication application)
			=> (App as IApplicationLifeCycle)?.OnPause();

		public override void WillTerminate(UIApplication application)
			=> (App as IApplicationLifeCycle).Dispose();

		public void Reload()
		{
			var oldVC = window.RootViewController;
			window.RootViewController = App.ToViewController();
			if(oldVC is CometViewController cvc)
			{
				if (cvc.CurrentView == App)
					cvc.CurrentView = null;

			}
			oldVC?.Dispose();
		}
	}
}

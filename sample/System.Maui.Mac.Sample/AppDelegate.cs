using AppKit;
using CoreGraphics;
using Foundation;
using System.Maui.Samples;
using System.Maui.Mac.Extensions;

namespace System.Maui.Mac.Sample
{
	[Register("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		private NSWindow _window;

		public AppDelegate()
		{
		}

		public override void DidFinishLaunching(NSNotification notification)
		{
			// Insert code here to initialize your application

			/*_window = new NSWindow(new CGRect(10, 10, 800, 600), NSWindowStyle.UnifiedTitleAndToolbar, NSBackingStore.Buffered, false);
            _window.ContentViewController = new MainPage().ToViewController();
            _window.IsVisible = true;
            _window.MakeKeyAndOrderFront(this);*/
#if DEBUG
			System.Maui.Reload.Init();
#endif
			System.Maui.Skia.UI.Init();

			//Replaces native controls with Skia Controls
			//System.Maui.Skia.Controls.Init ();
		}

		public override void WillTerminate(NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}

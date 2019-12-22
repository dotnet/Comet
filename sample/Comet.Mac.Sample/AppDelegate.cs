using AppKit;
using CoreGraphics;
using Foundation;
using Comet.Samples;
using Comet.Mac.Extensions;

namespace Comet.Mac.Sample
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
            Comet.Reload.Init();
#endif
            Comet.Skia.Mac.UI.Init();
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}

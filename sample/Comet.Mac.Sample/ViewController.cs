using System;

using AppKit;
using Foundation;
using Comet.Mac.Extensions;
using Comet.Samples;

namespace Comet.Mac.Sample
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var hotViewController = new MainPage().ToViewController();
            AddChildViewController(hotViewController);
            var view = hotViewController.View;
            view.AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;
            view.Frame = View.Bounds;

            View.AutoresizesSubviews = true;
            View.AddSubview(view);

            // Do any additional setup after loading the view.
        }



        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}

using System;
using AppKit;
using Comet.Mac.Extensions;

namespace Comet.Mac
{
    public class CometViewController : NSViewController
    {
        private CometView _containerView;

        public CometViewController()
        {
        }
        
        public View CurrentView
        {
            get => ContainerView.CurrentView;
            set => ContainerView.CurrentView = value;
        }

        public override void LoadView()
        {
            View = ContainerView;
        }
        
        private CometView ContainerView
        {
            get
            {
                if (_containerView == null)
                    _containerView = new CometView(NSScreen.MainScreen.Frame);

                return _containerView;
            }
        }
    }
}

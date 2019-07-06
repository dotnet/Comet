using System;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac
{
    public class HotUIViewController : NSViewController
    {
        private HotUIView _containerView;

        public HotUIViewController()
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
        
        private HotUIView ContainerView
        {
            get
            {
                if (_containerView == null)
                    _containerView = new HotUIView(NSScreen.MainScreen.Frame);

                return _containerView;
            }
        }
    }
}
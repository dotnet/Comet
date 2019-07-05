using CoreGraphics;
using UIKit;

namespace HotUI.iOS
{
    public class HotUIViewController : UIViewController
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
                    _containerView = new HotUIView(UIScreen.MainScreen.Bounds);

                return _containerView;
            }
        }
    }
}
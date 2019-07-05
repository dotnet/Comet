using System;
using CoreGraphics;
using UIKit;

namespace HotUI.iOS
{
    public class HotUIViewController : UIViewController
    {
        public HotUIViewController()
        {
        }

        IUIView currentView;

        public IUIView CurrentView
        {
            get => currentView;
            set
            {
                if (value == currentView)
                    return;
                currentView = value;
                if (currentView is ViewHandler vh)
                {
                    vh.ViewChanged = SetView;
                }

                SetView();
            }
        }

        UIView currentlyShownView;

        void SetView()
        {
            if (this.ViewIfLoaded == null || CurrentView == null)
                return;
            var view = CurrentView?.View;
            if (view == currentlyShownView)
                return;
            currentlyShownView?.RemoveFromSuperview();
            currentlyShownView = view;
            if (view == null)
                return;
            View.AddSubview(view);
        }

        public override void LoadView()
        {
            base.LoadView();
            View.BackgroundColor = UIColor.White;
            SetView();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            if (currentlyShownView == null)
                return;
            if (currentlyShownView is UIScrollView sv)
            {
                currentlyShownView.Frame = View.Bounds;
            }
            else
            {
                //TODO: opt out of safe are
                var bounds = View.Bounds;
                var safe = View.SafeAreaInsets;
                bounds.X += safe.Left;
                bounds.Y += safe.Top;
                bounds.Height -= safe.Top + safe.Bottom;
                bounds.Width -= safe.Left + safe.Right;
                
                if (currentlyShownView is UITableView lv)
                    currentlyShownView.Frame = bounds;
                else
                    currentlyShownView.Center = new CGPoint(bounds.GetMidX(), bounds.GetMidY());
            }
        }
    }
}
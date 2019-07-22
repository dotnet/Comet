using System;
using HotUI.iOS.Controls;
using UIKit;

// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS.Handlers
{
    public class ScrollViewHandler : AbstractHandler<ScrollView, UIScrollView>
    {
        private UIView _content;

        public override bool AutoSafeArea => false;
        protected override UIScrollView CreateView()
        {
            var scrollView = new UIScrollView()
            {
                ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Always
            };

            _content = VirtualView?.View?.ToView();
            if (_content != null)
            {
                if (VirtualView?.View != null)
                    VirtualView.View.NeedsLayout += HandleViewNeedsLayout;
                
                _content.SizeToFit();
                scrollView.Add(_content);
            }

            return scrollView;
        }

        private void HandleViewNeedsLayout(object sender, EventArgs e)
        {
            _content.SetNeedsLayout();
        }

        public override void Remove(View view)
        {
            if (VirtualView?.View != null)
                VirtualView.View.NeedsLayout -= HandleViewNeedsLayout;
            
            _content?.RemoveFromSuperview();
            _content = null;
            
            base.Remove(view);
        }
    }
}
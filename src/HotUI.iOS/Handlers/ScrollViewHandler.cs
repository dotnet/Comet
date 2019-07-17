using System;
using HotUI.iOS.Controls;
using UIKit;

// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS.Handlers
{
    public class ScrollViewHandler : AbstractHandler<ScrollView, UIScrollView>
    {
        private UIView _content;
        
        protected override UIScrollView CreateView()
        {
            var scrollView = new UIScrollView()
            {
                ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Always
            };

            _content = VirtualView?.View?.ToView();
            if (_content != null)
            {
                _content.SizeToFit();
                scrollView.Add(_content);
            }

            return scrollView;
        }
        
        public override void Remove(View view)
        {
            _content?.RemoveFromSuperview();
            _content = null;
            
            base.Remove(view);
        }
    }
}
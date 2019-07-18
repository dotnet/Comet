using System;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class ScrollViewHandler : AbstractControlHandler<ScrollView, NSScrollView>
    {
        public static readonly PropertyMapper<ScrollView> Mapper = new PropertyMapper<ScrollView>(ViewHandler.Mapper)
        {
            
        };

        private NSView _content;

        public ScrollViewHandler() : base(Mapper)
        {
        }
        
        protected override NSScrollView CreateView()
        {
            return new NSScrollView();
        }

        protected override void DisposeView(NSScrollView nativeView)
        {
            
        }

        public override void Remove(View view)
        {
            _content?.RemoveFromSuperview();
            base.Remove(view);
        }
        
        public override void SetView(View view)
        {
            base.SetView(view);

            var scroll = VirtualView;
            _content = scroll?.View?.ToView();
            if (_content != null)
            {
                //todo: fix this.  This is a hack to get the content to show up in the scrollview
                if (_content.Bounds.Width <= 0 && _content.Bounds.Height <= 0)
                    _content.Frame = new CoreGraphics.CGRect(0,0,800,600);

                var scrollView = (NSScrollView) NativeView;
                scrollView.DocumentView = _content;
            }
        }
    }
}
using System;
using AppKit;
using CoreGraphics;

namespace HotUI.Mac.Controls
{
    public class StackView : NSView
    {
        private bool _needsLayout;

        public StackView(CGRect rect) : base(rect)
        {
        }

        public StackView()
        {
        }

        public override bool IsFlipped => true;

        protected void LayoutSubviews()
        {
            var position = new CGPoint();

            foreach (var view in Subviews)
            {
                var bounds = view.Bounds;
                view.Frame = new CGRect(position, bounds.Size);
                position.Y += bounds.Height;
            }

            _needsLayout = false;
        }

        public override void WillRemoveSubview(NSView subview)
        {
            base.WillRemoveSubview(subview);
            _needsLayout = true;
        }

        public override void DidAddSubview(NSView subview)
        {
            base.DidAddSubview(subview);
            _needsLayout = true;
        }

        public override CGRect Frame
        {
            get => base.Frame;
            set
            {
                base.Frame = value;
                LayoutSubviews();
                _needsLayout = false;
            }
        }

        public override void ViewDidMoveToSuperview()
        {
            if (_needsLayout)
            {
                LayoutSubviews();
                _needsLayout = false;
            }

            base.ViewDidMoveToSuperview();
        }
    }
}

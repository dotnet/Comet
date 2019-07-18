using System;
using AppKit;
using CoreGraphics;
using HotUI.Mac.Controls;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class AbstractLayoutHandler : NSView, MacViewHandler
    {
        private AbstractLayout _view;
        private SizeF _measured;
        
        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        protected AbstractLayoutHandler(CGRect rect) : base(rect)
        {
            InitializeDefaults();
        }

        protected AbstractLayoutHandler()
        {
            InitializeDefaults();
        }
        
        public override bool IsFlipped => true;
        
        public NSView View => this;
        
        public HUIContainerView ContainerView => null;
        
        public object NativeView => View;
        
        public bool HasContainer
        {
            get => false; 
            set {}
        }        
        
        public SizeF Measure(SizeF available)
        {
            return available;
        }

        public void SetFrame(RectangleF frame)
        {
            View.Frame = frame.ToCGRect();
        }

        public void SetView(View view)
        {
            _view = view as AbstractLayout;
            if (_view != null)
            {
                _view.ChildrenChanged += HandleChildrenChanged;
                _view.ChildrenAdded += HandleChildrenAdded;
                _view.ChildrenRemoved += ViewOnChildrenRemoved;

                foreach (var subview in _view)
                {
                    subview.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
                    if (subview.ViewHandler is MacViewHandler handler)
                        handler.NativeViewChanged += HandleSubviewNativeViewChanged;
                    
                    var nativeView = subview.ToView() ?? new NSView();
                    AddSubview(nativeView);
                }

                SetNeedsLayout();
            }
        }

        public void Remove(View view)
        {
            foreach (var subview in _view)
            {
                subview.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
                if (subview.ViewHandler is MacViewHandler handler)
                    handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
            }
            
            foreach (var subview in Subviews)
                subview.RemoveFromSuperview();

            if (view != null)
            {
                _view.ChildrenChanged -= HandleChildrenChanged;
                _view.ChildrenAdded -= HandleChildrenAdded;
                _view.ChildrenRemoved -= ViewOnChildrenRemoved;
                _view = null;
            }
        }
        
        private void HandleSubviewViewHandlerChanged(object sender, ViewHandlerChangedEventArgs e)
        {
            if (e.OldViewHandler is MacViewHandler oldHandler)
                oldHandler.NativeViewChanged -= HandleSubviewNativeViewChanged;
        }

        private void HandleSubviewNativeViewChanged(object sender, ViewChangedEventArgs args)
        {
            args.OldNativeView?.RemoveFromSuperview();

            var index = _view.IndexOf(args.VirtualView);
            var newView = args.NewNativeView ?? new NSView();
            this.InsertSubview(newView, index);
        }

        public virtual void UpdateValue(string property, object value)
        {
        }

        private void InitializeDefaults()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
        }

        private void HandleChildrenAdded(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var view = _view[index];
                
                view.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
                if (view.ViewHandler is MacViewHandler handler)
                    handler.NativeViewChanged += HandleSubviewNativeViewChanged;
                
                var nativeView = view.ToView() ?? new NSView();
                this.InsertSubview(nativeView, index);
            }

            SetNeedsLayout();
        }

        private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
        {
            if (e.Removed != null)
            {
                foreach (var view in e.Removed)
                {
                    view.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
                    if (view.ViewHandler is MacViewHandler handler)
                        handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
                }
            }
            
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var nativeView = Subviews[index];
                nativeView.RemoveFromSuperview();
            }

            SetNeedsLayout();
        }

        private void HandleChildrenChanged(object sender, LayoutEventArgs e)
        {
            if (e.Removed != null)
            {
                foreach (var view in e.Removed)
                {
                    view.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
                    if (view.ViewHandler is MacViewHandler handler)
                        handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
                }
            }
            
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var oldNativeView = Subviews[index];
                oldNativeView.RemoveFromSuperview();

                var view = _view[index];
                
                view.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
                if (view.ViewHandler is MacViewHandler handler)
                    handler.NativeViewChanged += HandleSubviewNativeViewChanged;
                
                var newNativeView = view.ToView() ?? new NSView();
                this.InsertSubview(newNativeView, index);
            }

            SetNeedsLayout();
        }

        private void SetNeedsLayout()
        {
            NeedsLayout = true;
        }

        public CGSize SizeThatFits(CGSize size)
        {
            _measured = _view.Measure(size.ToSizeF());
            return _measured.ToCGSize();
        }

        public void SizeToFit()
        {
            _measured = _view.Measure(Superview?.Bounds.Size.ToSizeF() ?? NSScreen.MainScreen.Frame.Size.ToSizeF());
            base.Frame = new CGRect(new CGPoint(0, 0), _measured.ToCGSize());
        }

        public override CGSize IntrinsicContentSize => _measured.ToCGSize();
        
        public override CGRect Frame
        {
            get => base.Frame;
            set
            {
                base.Frame = value;
                Layout();
                NeedsLayout = false;
            }
        }
        
        public override void ViewDidMoveToSuperview()
        {
            if (NeedsLayout)
            {
                Layout();
                NeedsLayout = false;
            }

            base.ViewDidMoveToSuperview();
        }
        
        public override void Layout()
        {
            if (Superview == null || Bounds.Size.IsEmpty)
                return;

            _view.Frame = Frame.ToRectangleF();
        }
    }
}
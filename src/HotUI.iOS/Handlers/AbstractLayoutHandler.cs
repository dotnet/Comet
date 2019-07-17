using System;
using System.Collections.Generic;
using CoreGraphics;
using HotUI.iOS.Controls;
using HotUI.Layout;
using UIKit;

namespace HotUI.iOS.Handlers
{
    public class AbstractLayoutHandler : UIView, iOSViewHandler, ILayoutHandler<UIView>
    {
        public static int _counter = 0;
        public int _instance;

        private AbstractLayout _view;
        private SizeF _measured;
        private bool _measurementValid;

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        protected AbstractLayoutHandler(CGRect rect) : base(rect)
        {
            _instance = _counter++;
            InitializeDefaults();
        }

        protected AbstractLayoutHandler()
        {
            _instance = _counter++;
        }
        
        public SizeF Measure(UIView view, SizeF available)
        {
            CGSize size;
            if (view is AbstractLayoutHandler || view is HUIContainerView)
            {
                size = view.SizeThatFits(available.ToCGSize());
            }
            else
            {
                size = view.IntrinsicContentSize;
                if (size.Width == 0 || size.Height == 0)
                    size = view.Bounds.Size;
            }

            return size.ToSizeF();
        }

        public SizeF GetSize(UIView view)
        {
            var size = view.Bounds.Size;
            if (size.Width == 0 || size.Height == 0)
                size = view.IntrinsicContentSize;

            return size.ToSizeF();
        }

        public void SetFrame(UIView view, float x, float y, float width, float height)
        {
            view.Frame = new CGRect(x, y, width, height);
        }

        public void SetSize(UIView view, float width, float height)
        {
            if ((Equals(width, (float)Frame.Width) && Equals(height, (float)Frame.Height)))
                return;

            view.Frame = new CGRect(Frame.X, Frame.Y, width, height);
        }

        public IEnumerable<UIView> GetSubviews()
        {
            return Subviews;
        }

        public UIView View => this;

        public HUIContainerView ContainerView => null;

        public object NativeView => this;

        public bool HasContainer
        {
            get => false; 
            set {}
        }

        public SizeF Measure(SizeF availableSize)
        {
            return availableSize;
        }

        public void SetFrame(RectangleF frame)
        {
            View.Frame = frame.ToCGRect();
        }

        public void SetView(View view)
        {
            Console.WriteLine($"[{GetType().Name} - {_instance}] SetView({view.GetType().Name})");

            if (_view != null)
                Console.WriteLine("Removed should have been called beforehand.");

            _view = view as AbstractLayout;
            if (_view != null)
            {
                _view.ChildrenChanged += HandleChildrenChanged;
                _view.ChildrenAdded += HandleChildrenAdded;
                _view.ChildrenRemoved += ViewOnChildrenRemoved;

                foreach (var subview in _view)
                {
                    subview.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
                    if (subview.ViewHandler is iOSViewHandler handler)
                        handler.NativeViewChanged += HandleSubviewNativeViewChanged;

                    var nativeView = subview.ToView() ?? new UIView();
                    AddSubview(nativeView);
                }

                SetNeedsLayout();
                _measurementValid = false;
            }
        }

        public void Remove(View view)
        {
            Console.WriteLine($"[{GetType().Name} - {_instance}] Remove({view.GetType().Name})");

            foreach (var subview in _view)
            {
                subview.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
                if (subview.ViewHandler is iOSViewHandler handler)
                    handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
            }

            foreach (var subview in Subviews)
            {
                subview.RemoveFromSuperview();
            }

            _view.ChildrenChanged -= HandleChildrenChanged;
            _view.ChildrenAdded -= HandleChildrenAdded;
            _view.ChildrenRemoved -= ViewOnChildrenRemoved;
            _view = null;
        }

        private void HandleSubviewViewHandlerChanged(object sender, ViewHandlerChangedEventArgs e)
        {
            Console.WriteLine($"[{GetType().Name} - {_instance}] HandleSubviewViewHandlerChanged: [{sender.GetType()}] From:[{e.OldViewHandler?.GetType()}] To:[{e.NewViewHandler?.GetType()}]");

            if (e.OldViewHandler is iOSViewHandler oldHandler)
                oldHandler.NativeViewChanged -= HandleSubviewNativeViewChanged;
        }

        private void HandleSubviewNativeViewChanged(object sender, ViewChangedEventArgs args)
        {
            Console.WriteLine($"[{GetType().Name} - {_instance}] HandlerViewChanged: [{sender.GetType()}] From:[{args.OldNativeView?.GetType()}] To:[{args.NewNativeView?.GetType()}]");

            args.OldNativeView?.RemoveFromSuperview();

            var index = _view.IndexOf(args.VirtualView);
            var newView = args.NewNativeView ?? new UIView();
            InsertSubview(newView, index);
        }
        
        public virtual void UpdateValue(string property, object value)
        {
        }

        private void InitializeDefaults()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            BackgroundColor = UIColor.Green;
        }

        private void HandleChildrenAdded(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var view = _view[index];

                view.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
                if (view.ViewHandler is iOSViewHandler handler)
                    handler.NativeViewChanged += HandleSubviewNativeViewChanged;

                var nativeView = view.ToView() ?? new UIView();
                InsertSubview(nativeView, index);
            }

            SetNeedsLayout();
            _measurementValid = false;
        }

        private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
        {
            if (e.Removed != null)
            {
                foreach (var view in e.Removed)
                {
                    view.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
                    if (view.ViewHandler is iOSViewHandler handler)
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
            _measurementValid = false;
        }

        private void HandleChildrenChanged(object sender, LayoutEventArgs e)
        {
            if (e.Removed != null)
            {
                foreach (var view in e.Removed)
                {
                    view.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
                    if (view.ViewHandler is iOSViewHandler handler)
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
                if (view.ViewHandler is iOSViewHandler handler)
                    handler.NativeViewChanged += HandleSubviewNativeViewChanged;

                var newNativeView = view.ToView() ?? new UIView();
                InsertSubview(newNativeView, index);
            }

            SetNeedsLayout();
            _measurementValid = false;
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            _measured = _view.Measure(size.ToSizeF());
            return _measured.ToCGSize();
        }

        public override void SizeToFit()
        {
            _measured = _view.Measure(Superview?.Bounds.Size.ToSizeF() ?? UIScreen.MainScreen.Bounds.Size.ToSizeF());
            _measurementValid = true;
            base.Frame = new CGRect(new CGPoint(0, 0), _measured.ToCGSize());
        }

        public override CGSize IntrinsicContentSize => _measured.ToCGSize();
        
        public override void LayoutSubviews()
        {
            if (Superview == null || Bounds.Size.IsEmpty)
                return;

            _view.Frame = Frame.ToRectangleF();
        }
    }
}
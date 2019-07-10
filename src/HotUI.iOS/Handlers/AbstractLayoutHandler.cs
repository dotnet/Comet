using System;
using System.Collections.Generic;
using System.Drawing;
using CoreGraphics;
using HotUI.iOS.Controls;
using HotUI.Layout;
using UIKit;

namespace HotUI.iOS
{
    public class AbstractLayoutHandler : UIView, iOSViewHandler, ILayoutHandler<UIView>
    {
        private readonly ILayoutManager<UIView> _layoutManager;
        private AbstractLayout _view;
        private Size _measured;
        private bool _measurementValid;

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;
        public event EventHandler RemovedFromView;

        protected AbstractLayoutHandler(CGRect rect, ILayoutManager<UIView> layoutManager) : base(rect)
        {
            _layoutManager = layoutManager;
            InitializeDefaults();
        }

        protected AbstractLayoutHandler(ILayoutManager<UIView> layoutManager)
        {
            _layoutManager = layoutManager;
        }

        public AbstractLayout Layout => _view;

        public Size Measure(UIView view, Size available)
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

            return size.ToHotUISize();
        }

        public Size GetSize(UIView view)
        {
            var size = view.Bounds.Size;
            if (size.Width == 0 || size.Height == 0)
                size = view.IntrinsicContentSize;

            return size.ToHotUISize();
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

        public void SetView(View view)
        {
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
                    if (subview.ViewHandler is iOSViewHandler handler)
                    {
                        handler.RemovedFromView += HandleHandlerRemovedFromView;
                        handler.NativeViewChanged += HandleSubviewChanged;
                    }

                    var nativeView = subview.ToView() ?? new UIView();
                    AddSubview(nativeView);
                }

                SetNeedsLayout();
                _measurementValid = false;
            }
        }

        public void Remove(View view)
        {
            foreach (var subview in _view)
            {
                if (subview.ViewHandler is iOSViewHandler handler)
                {
                    handler.RemovedFromView -= HandleHandlerRemovedFromView;
                    handler.NativeViewChanged -= HandleSubviewChanged;
                }
            }

            foreach (var subview in Subviews)
            {
                subview.RemoveFromSuperview();
            }

            _view.ChildrenChanged -= HandleChildrenChanged;
            _view.ChildrenAdded -= HandleChildrenAdded;
            _view.ChildrenRemoved -= ViewOnChildrenRemoved;
            _view = null;

            RemovedFromView?.Invoke(this, EventArgs.Empty);
        }

        private void HandleHandlerRemovedFromView(object sender, EventArgs e)
        {
            var handler = (iOSViewHandler)sender;
            handler.RemovedFromView -= HandleHandlerRemovedFromView;
            handler.NativeViewChanged -= HandleSubviewChanged;
        }

        private void HandleSubviewChanged(object sender, ViewChangedEventArgs args)
        {
            Console.WriteLine($"HandlerViewChanged: [{sender.GetType()}] From:[{args.OldNativeView.GetType()}] To:[{args.NewNativeView.GetType()}]");

            if (args.OldNativeView != null)
                args.OldNativeView.RemoveFromSuperview();

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

                if (view.ViewHandler is iOSViewHandler handler)
                {
                    handler.RemovedFromView += HandleHandlerRemovedFromView;
                    handler.NativeViewChanged += HandleSubviewChanged;
                }

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
                    if (view.ViewHandler is iOSViewHandler handler)
                    {
                        handler.RemovedFromView -= HandleHandlerRemovedFromView;
                        handler.NativeViewChanged -= HandleSubviewChanged;
                    }
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
                    if (view.ViewHandler is iOSViewHandler handler)
                    {
                        handler.RemovedFromView -= HandleHandlerRemovedFromView;
                        handler.NativeViewChanged -= HandleSubviewChanged;
                    }
                }
            }

            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var oldNativeView = Subviews[index];
                oldNativeView.RemoveFromSuperview();

                var view = _view[index];

                if (view.ViewHandler is iOSViewHandler handler)
                {
                    handler.RemovedFromView += HandleHandlerRemovedFromView;
                    handler.NativeViewChanged += HandleSubviewChanged;
                }

                var newNativeView = view.ToView() ?? new UIView();
                InsertSubview(newNativeView, index);
            }

            SetNeedsLayout();
            _measurementValid = false;
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            _measured = _layoutManager.Measure(this, this, _view, size.ToHotUISize());
            _measurementValid = true;
            return _measured.ToCGSize();
        }

        public override void SizeToFit()
        {
            _measured = _layoutManager.Measure(this, this, _view, Superview?.Bounds.Size.ToHotUISize() ?? UIScreen.MainScreen.Bounds.Size.ToHotUISize());
            _measurementValid = true;
            base.Frame = new CGRect(new CGPoint(0, 0), _measured.ToCGSize());
        }

        public override CGSize IntrinsicContentSize => _measured.ToCGSize();

        public override void LayoutSubviews()
        {
            if (Superview == null || Bounds.Size.IsEmpty)
                return;

            var available = Bounds.Size.ToHotUISize();
            if (!_measurementValid)
            {
                _measured = _layoutManager.Measure(this, this, _view, available);
                _measurementValid = true;
            }

            _layoutManager.Layout(this, this, _view, _measured);
        }
    }
}
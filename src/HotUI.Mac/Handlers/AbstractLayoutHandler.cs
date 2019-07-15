using System.Collections.Generic;
using CoreGraphics;
using HotUI.Layout;
using AppKit;
using HotUI.Mac.Controls;
using HotUI.Mac.Extensions;

namespace HotUI.Mac
{
    public class AbstractLayoutHandler : NSView, MacViewHandler, ILayoutHandler<NSView>
    {
        private readonly ILayoutManager<NSView> _layoutManager;
        private AbstractLayout _view;
        private SizeF _measured;
        private bool _measurementValid;

        public AbstractLayout LayoutView => _view;
        
        public HUIContainerView ContainerView => null;

        public override bool IsFlipped => true;

        protected AbstractLayoutHandler(CGRect rect, ILayoutManager<NSView> layoutManager) : base(rect)
        {
            _layoutManager = layoutManager;
            InitializeDefaults();
        }

        protected AbstractLayoutHandler(ILayoutManager<NSView> layoutManager)
        {
            _layoutManager = layoutManager;
            InitializeDefaults();
        }

        public SizeF Measure(NSView view, SizeF available)
        {
            CGSize size;
            if (view is AbstractLayoutHandler handler)
            {
                size = handler.SizeThatFits(available.ToCGSize());
            }
            else
            {
                size = view.IntrinsicContentSize;
                if (size.Width == 0 || size.Height == 0)
                    size = view.Bounds.Size;
            }

            return size.ToHotUISize();
        }

        public SizeF GetSize(NSView view)
        {
            var size = view.Bounds.Size;
            if (size.Width == 0 || size.Height == 0)
                size = view.IntrinsicContentSize;

            return size.ToHotUISize();
        }

        public void SetFrame(NSView view, float x, float y, float width, float height)
        {
            view.Frame = new CGRect(x, y, width, height);
        }

        public void SetSize(NSView view, float width, float height)
        {
            if ((Equals(width, (float)Frame.Width) && Equals(height, (float)Frame.Height)))
                return;

            view.Frame = new CGRect(Frame.X, Frame.Y, width, height);
        }

        public IEnumerable<NSView> GetSubviews()
        {
            return Subviews;
        }

        public NSView View => this;
        
        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        public void SetView(View view)
        {
            _view = view as AbstractLayout;
            if (_view != null)
            {
                _view.ChildrenChanged += HandleChildrenChanged;
                _view.ChildrenAdded += HandleChildrenAdded;
                _view.ChildrenRemoved += ViewOnChildrenRemoved;

                foreach (var subView in _view)
                {
                    var nativeView = subView.ToView() ?? new NSView();
                    AddSubview(nativeView);
                }

                SetNeedsLayout();
                _measurementValid = false;
            }
        }

        public void Remove(View view)
        {
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
                var nativeView = view.ToView() ?? new NSView();
                this.InsertSubview(nativeView, index);
            }

            SetNeedsLayout();
            _measurementValid = false;
        }

        private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
        {
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
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var oldNativeView = Subviews[index];
                oldNativeView.RemoveFromSuperview();

                var view = _view[index];
                var newNativeView = view.ToView() ?? new NSView();
                this.InsertSubview(newNativeView, index);
            }

            SetNeedsLayout();
            _measurementValid = false;
        }

        private void SetNeedsLayout()
        {
            NeedsLayout = true;
        }

        public CGSize SizeThatFits(CGSize size)
        {
            _measured = _layoutManager.Measure(this, this, _view, size.ToHotUISize());
            _measurementValid = true;
            return _measured.ToCGSize();
        }

        public void SizeToFit()
        {
            _measured = _layoutManager.Measure(this, this, _view, Superview?.Bounds.Size.ToHotUISize() ?? NSScreen.MainScreen.Frame.Size.ToHotUISize());
            _measurementValid = true;
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
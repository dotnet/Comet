using System;
using Android.Runtime;
using HotUI.Android.Controls;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;

namespace HotUI.Android.Handlers
{
    public class AbstractLayoutHandler : AViewGroup, AndroidViewHandler
    {
        private AbstractLayout _view;

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        protected AbstractLayoutHandler() : base(AndroidContext.CurrentContext)
        {

        }

        protected AbstractLayoutHandler(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference,  transfer)
        {

        }

        public AView View => this;

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
            Layout((int)frame.Left, (int)frame.Top, (int)frame.Right, (int)frame.Bottom);
        }

        public void SetView(View view)
        {
            _view = view as AbstractLayout;
            if (_view != null)
            {
                _view.NeedsLayout += HandleNeedsLayout;
                _view.ChildrenChanged += HandleChildrenChanged;
                _view.ChildrenAdded += HandleChildrenAdded;
                _view.ChildrenRemoved += ViewOnChildrenRemoved;

                foreach (var subview in _view)
                {
                    subview.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
                    if (subview.ViewHandler is AndroidViewHandler handler)
                        handler.NativeViewChanged += HandleSubviewNativeViewChanged;

                    var nativeView = subview.ToView() ?? new AView(AndroidContext.CurrentContext);
                    AddView(nativeView);
                }

                Invalidate();
            }
        }

        private void HandleNeedsLayout(object sender, EventArgs e)
        {
            Invalidate();
        }

        public void Remove(View view)
        {
            foreach (var subview in _view)
            {
                subview.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
                if (subview.ViewHandler is AndroidViewHandler handler)
                    handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
            }
    
            _view.NeedsLayout -= HandleNeedsLayout;
            _view.ChildrenChanged -= HandleChildrenChanged;
            _view.ChildrenAdded -= HandleChildrenAdded;
            _view.ChildrenRemoved -= ViewOnChildrenRemoved;
            _view = null;
        }

        private void HandleSubviewViewHandlerChanged(object sender, ViewHandlerChangedEventArgs e)
        {
            if (e.OldViewHandler is AndroidViewHandler oldHandler)
                oldHandler.NativeViewChanged -= HandleSubviewNativeViewChanged;
        }

        private void HandleSubviewNativeViewChanged(object sender, ViewChangedEventArgs args)
        {
            if (args.OldNativeView != null)
            {
                if (args.OldNativeView.Parent is AViewGroup parent)
                    parent.RemoveView(args.OldNativeView);
            }

            var index = _view.IndexOf(args.VirtualView);
            var newView = args.NewNativeView ?? new AView(AndroidContext.CurrentContext);
            AddView(newView, index);
        }
        
        public virtual void UpdateValue(string property, object value)
        {
        }

        private void HandleChildrenAdded(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var view = _view[index];

                view.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
                if (view.ViewHandler is AndroidViewHandler handler)
                    handler.NativeViewChanged += HandleSubviewNativeViewChanged;

                var nativeView = view.ToView() ?? new AView(AndroidContext.CurrentContext);
                AddView(nativeView, index);
            }

            Invalidate();
        }

        private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
        {
            if (e.Removed != null)
            {
                foreach (var view in e.Removed)
                {
                    view.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
                    if (view.ViewHandler is AndroidViewHandler handler)
                        handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
                }
            }

            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                RemoveViewAt(index);
            }

            Invalidate();
        }

        private void HandleChildrenChanged(object sender, LayoutEventArgs e)
        {
            if (e.Removed != null)
            {
                foreach (var view in e.Removed)
                {
                    view.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
                    if (view.ViewHandler is AndroidViewHandler handler)
                        handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
                }
            }

            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                RemoveViewAt(index);

                var view = _view[index];

                view.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
                if (view.ViewHandler is AndroidViewHandler handler)
                    handler.NativeViewChanged += HandleSubviewNativeViewChanged;

                var newNativeView = view.ToView() ?? new AView(AndroidContext.CurrentContext);
                AddView(newNativeView, index);
            }

            Invalidate();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
            var heightMode = MeasureSpec.GetMode(heightMeasureSpec);

            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = MeasureSpec.GetSize(heightMeasureSpec);

            var measured = _view.Measure(new SizeF(width, height));
            SetMeasuredDimension((int)measured.Width, (int)measured.Height);

        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            if (_view == null) return;

            var width = right - left;
            var height = bottom - top;

            if (width > 0 && height > 0)
                _view.Frame = new RectangleF(left, top, width, height);
        }       
    }
}
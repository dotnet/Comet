using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPFSize = System.Windows.Size;

namespace HotUI.WPF.Handlers
{
    public abstract class AbstractLayoutHandler : Panel, WPFViewHandler
    {
        private AbstractLayout _view;

        public IEnumerable<UIElement> GetSubviews()
        {
            foreach (var element in InternalChildren) yield return (UIElement) element;
        }

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        public UIElement View => this;

        public object NativeView => View;

        public bool HasContainer
        {
            get => false;
            set { }
        }

        public SizeF Measure(SizeF availableSize)
        {
            return availableSize;
        }

        public void SetFrame(RectangleF frame)
        {
            Arrange(frame.ToRect());

            /*Canvas.SetLeft(this, frame.Left);
            Canvas.SetTop(this, frame.Top);

            Width = frame.Width;
            Height = frame.Height;*/
        }

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
                    var nativeView = subView.ToView();
                    InternalChildren.Add(nativeView);
                }

                InvalidateArrange();
            }
        }

        public void Remove(View view)
        {
            InternalChildren.Clear();

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

        private void HandleChildrenAdded(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var view = _view[index];
                var nativeView = view.ToView();
                InternalChildren.Insert(index, nativeView);
            }

            InvalidateArrange();
        }

        private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                InternalChildren.RemoveAt(index);
            }

            InvalidateArrange();
        }

        private void HandleChildrenChanged(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                InternalChildren.RemoveAt(index);

                var view = _view[index];
                var newNativeView = view.ToView();
                InternalChildren.Insert(index, newNativeView);
            }

            InvalidateArrange();
        }

        private void LayoutSubviews()
        {
            if (Width > 0 && Height > 0)
                _view.Frame = new RectangleF(0,0, (float)Width, (float)Height);
        }

        protected override WPFSize MeasureOverride(WPFSize availableSize)
        {
            var size = new WPFSize();

            for (var i = 0; i < InternalChildren.Count; i++)
            {
                var child = InternalChildren[i];

                if (child is System.Windows.Controls.ListView listView)
                {
                    var sizeToUse = GetMeasuredSize(listView, availableSize.ToSizeF());
                    child.Measure(sizeToUse.ToSize());
                    size.Height = Math.Max(child.DesiredSize.Height, size.Height);
                    size.Width = Math.Max(child.DesiredSize.Width, size.Width);
                }
                else
                {
                    child.Measure(availableSize);
                    size.Height = Math.Max(child.DesiredSize.Height, size.Height);
                    size.Width = Math.Max(child.DesiredSize.Width, size.Width);
                }
            }

            return size;
        }

        protected virtual SizeF GetMeasuredSize(UIElement child, SizeF availableSize)
        {
            return availableSize;
        }

        private bool _inArrange = false;

        protected override WPFSize ArrangeOverride(WPFSize finalSize)
        {
            if (_inArrange)
                return finalSize;

            _inArrange = true;
            RenderSize = finalSize;
            Width = finalSize.Width;
            Height = finalSize.Height;
            if (finalSize.Width > 0 && finalSize.Height > 0)
                LayoutSubviews();
            _inArrange = false;

            return finalSize;
        }

        public SizeF Measure(UIElement view, SizeF available)
        {
            view.Measure(available.ToSize());
            return view.DesiredSize.ToSizeF();
        }
    }
}
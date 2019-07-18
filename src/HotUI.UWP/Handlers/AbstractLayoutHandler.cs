using System;
using System.Collections.Generic;
using System.Drawing;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HotUI.Layout;
using UwpSize = Windows.Foundation.Size;

namespace HotUI.UWP.Handlers
{
    public abstract class AbstractLayoutHandler : Canvas, UWPViewHandler
    {
        private AbstractLayout _view;

        public IEnumerable<UIElement> GetSubviews()
        {
            foreach (var element in Children) yield return element;
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

           /* Canvas.SetLeft(this, frame.Left);
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
                    Children.Add(nativeView);
                }

                LayoutSubviews();
            }
        }

        public void Remove(View view)
        {
            Children.Clear();

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
                Children.Insert(index, nativeView);
            }

            LayoutSubviews();
        }

        private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                Children.RemoveAt(index);
            }

            LayoutSubviews();
        }

        private void HandleChildrenChanged(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                Children.RemoveAt(index);

                var view = _view[index];
                var newNativeView = view.ToView();
                Children.Insert(index, newNativeView);
            }

            LayoutSubviews();
        }

        private void LayoutSubviews()
        {
            _view.Frame = new RectangleF(0,0,(float)Width, (float)Height);
        }

        protected override UwpSize MeasureOverride(UwpSize availableSize)
        {
            var size = new UwpSize();

            foreach (var child in Children)
            {
                if (child is Windows.UI.Xaml.Controls.ListView listView)
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

        protected override UwpSize ArrangeOverride(UwpSize finalSize)
        {

            Width = finalSize.Width;
            Height = finalSize.Height;
            if (finalSize.Width > 0 && finalSize.Height > 0)
                LayoutSubviews();

            return finalSize;
        }
    }
}
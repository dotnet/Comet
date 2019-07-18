using System;
using System.Windows;
using System.Windows.Controls;

namespace HotUI.WPF.Handlers
{
    public abstract class AbstractStackLayoutHandler : StackPanel, WPFViewHandler
    {
        private AbstractLayout _view;

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
            Measure(availableSize.ToSize());
            return DesiredSize.ToSizeF();
        }

        public void SetFrame(RectangleF frame)
        {
            Canvas.SetLeft(this, frame.Left);
            Canvas.SetTop(this, frame.Top);

            Width = frame.Width;
            Height = frame.Height;
        }

        public void SetView(View view)
        {
            System.Diagnostics.Debug.WriteLine($"v SetView [{GetType().Name}] [{view.Id}]");

            _view = view as AbstractLayout;
            if (_view != null)
            {
                _view.ChildrenChanged += HandleChildrenChanged;
                _view.ChildrenAdded += HandleChildrenAdded;
                _view.ChildrenRemoved += ViewOnChildrenRemoved;

                foreach (var subView in _view)
                {
                    var nativeView = subView.ToView();
                    if (!Children.Contains(nativeView))
                    {
                        System.Diagnostics.Debug.WriteLine("  -Adding native view to parent.");
                        Children.Add(nativeView);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("  -Parent already contains native view. Skipping");
                    }

                    System.Diagnostics.Debug.WriteLine($"    -Child [{subView.Id}] of type [{subView.GetType().Name}]");
                    System.Diagnostics.Debug.WriteLine($"    -Parent [{_view.Id}] of type [{_view.GetType().Name}]");
                }
            }

            System.Diagnostics.Debug.WriteLine($"^ SetView");

        }

        public void Remove(View view)
        {
            System.Diagnostics.Debug.WriteLine($"v Remove [{GetType().Name}] [{view.Id}]");

            Children.Clear();

            if (view != null)
            {
                _view.ChildrenChanged -= HandleChildrenChanged;
                _view.ChildrenAdded -= HandleChildrenAdded;
                _view.ChildrenRemoved -= ViewOnChildrenRemoved;
                _view = null;
            }

            System.Diagnostics.Debug.WriteLine($"^ Remove");
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
        }

        private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                Children.RemoveAt(index);
            }
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
        }
    }
}
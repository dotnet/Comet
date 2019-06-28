using System.Collections.Generic;
using System.Drawing;
using CoreGraphics;
using HotUI.Layout;
using UIKit;

namespace HotUI.iOS
{
    public class AbstractLayoutHandler : UIView, IUIView, ILayoutHandler<UIView>
    {
        private readonly ILayoutManager<UIView> _layoutManager;
        private AbstractLayout _view;

        protected AbstractLayoutHandler(CGRect rect, ILayoutManager<UIView> layoutManager) : base(rect)
        {
            _layoutManager = layoutManager;
            InitializeDefaults();
        }

        protected AbstractLayoutHandler(ILayoutManager<UIView> layoutManager)
        {
            _layoutManager = layoutManager;
        }

        public SizeF GetSize(UIView view)
        {
            return view.Bounds.Size.ToSizeF();
        }

        public void SetFrame(UIView view, float x, float y, float width, float height)
        {
            view.Frame = new CGRect(x, y, width, height);
        }

        public void SetSize(UIView view, float width, float height)
        {
            view.Frame = new CGRect(Frame.X, Frame.Y, width, height);
        }

        public IEnumerable<UIView> GetSubviews()
        {
            return Subviews;
        }

        public UIView View => this;

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
                    AddSubview(nativeView);
                }

                LayoutSubviews();
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
            BackgroundColor = UIColor.Green;
        }

        private void HandleChildrenAdded(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var view = _view[index];
                var nativeView = view.ToView();
                InsertSubview(nativeView, index);
            }

            LayoutSubviews();
        }

        private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var nativeView = Subviews[index];
                nativeView.RemoveFromSuperview();
            }

            LayoutSubviews();
        }

        private void HandleChildrenChanged(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                var oldNativeView = Subviews[index];
                oldNativeView.RemoveFromSuperview();

                var view = _view[index];
                var newNativeView = view.ToView();
                InsertSubview(newNativeView, index);
            }

            LayoutSubviews();
        }

        public override void LayoutSubviews()
        {
            _layoutManager.Layout(this, this, _view);
        }
    }
}
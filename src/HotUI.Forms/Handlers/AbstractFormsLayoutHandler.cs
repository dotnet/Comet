

namespace HotUI.Forms
{
    public abstract class AbstractFormsLayoutHandler : IFormsView
    {
        private readonly Xamarin.Forms.Layout<Xamarin.Forms.View> _formsLayout;
        private AbstractLayout _view;

        protected AbstractFormsLayoutHandler(Xamarin.Forms.Layout<Xamarin.Forms.View> layout)
        {
            _formsLayout = layout;
        }

        public Xamarin.Forms.View View => _formsLayout;

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
                    var nativeView = subView.ToForms() ?? new ContentView();
                    _formsLayout.Children.Add(nativeView);
                }
            }
        }

        public void Remove(View view)
        {
            _formsLayout.Children.Clear();

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
                var nativeView = view.ToForms() ?? new ContentView();
                _formsLayout.Children.Insert(index, nativeView);
            }
        }

        private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                _formsLayout.Children.RemoveAt(index);
            }
        }

        private void HandleChildrenChanged(object sender, LayoutEventArgs e)
        {
            for (var i = 0; i < e.Count; i++)
            {
                var index = e.Start + i;
                _formsLayout.Children.RemoveAt(index);

                var view = _view[index];
                var newNativeView = view.ToForms();
                _formsLayout.Children.Insert(index, newNativeView);
            }
        }
    }
}
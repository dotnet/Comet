using HotUI.Forms.Handlers;

namespace HotUI.Forms
{
    public class HotUIContainerView : Xamarin.Forms.ContentView
    {
        private View _view;
        private Xamarin.Forms.View _nativeView;
        private IViewHandler _handler;

        public HotUIContainerView(View view = null)
        {
            View = view;
        }

        public View View
        {
            get => _view;
            set
            {
                if (value == _view)
                    return;

                if (_handler is ViewHandler oldViewHandler)
                    oldViewHandler.NativeViewChanged -= HandleNativeViewChanged;

                _view = value;
                _handler = _view?.ViewHandler;

                if (_handler is ViewHandler newViewHandler)
                    newViewHandler.NativeViewChanged += HandleNativeViewChanged;

                HandleNativeViewChanged(this, null);
            }
        }

        private void HandleNativeViewChanged(object sender, ViewChangedEventArgs e)
        {
            if (_nativeView != null)
            {
                Content = null;
                _nativeView = null;
            }

            _nativeView = _view?.GetOrCreateViewHandler()?.View;

            if (_nativeView != null)
                Content = _nativeView;
        }
    }
}

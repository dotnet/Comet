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
                    oldViewHandler.ViewChanged = null;

                _view = value;
                _handler = _view?.ViewHandler;

                if (_handler is ViewHandler newViewHandler)
                    newViewHandler.ViewChanged = UpdateNativeView;

                UpdateNativeView();
            }
        }

        private void UpdateNativeView()
        {
            if (_nativeView != null)
            {
                Content = null;
                _nativeView = null;
            }

            _nativeView = _view?.ToIFormsView()?.View;

            if (_nativeView != null)
                Content = _nativeView;
        }
    }
}

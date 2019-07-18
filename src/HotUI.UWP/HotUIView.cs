using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using HotUI.UWP.Handlers;

namespace HotUI.UWP
{
    public class HotUIView : Grid
    {
        private View _view;
        private UIElement _nativeView;
        private IViewHandler _handler;

        public HotUIView(View view = null)
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
                Children.Remove(_nativeView);
                _nativeView = null;
            }

            _nativeView = _view?.ToView();

            if (_nativeView != null)
            {
                Children.Add(_nativeView);
            }
        }
    }
}

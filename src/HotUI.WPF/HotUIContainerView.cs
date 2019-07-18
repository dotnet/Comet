using System.Windows;
using System.Windows.Controls;
using HotUI.WPF.Handlers;

namespace HotUI.WPF
{
    public class HotUIContainerView : Grid
    {
        private View _view;
        private UIElement _nativeView;
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
                Children.Remove(_nativeView);
                _nativeView = null;
            }

            _nativeView = _view?.ToView();

            if (_nativeView != null)
            {
                Grid.SetRow(_nativeView, 0);
                Grid.SetColumn(_nativeView, 0);
                Grid.SetColumnSpan(_nativeView, 1);
                Grid.SetRowSpan(_nativeView, 1);

                Children.Add(_nativeView);
            }
        }
    }
}

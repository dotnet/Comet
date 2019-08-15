using System.Windows;
using System.Windows.Controls;
using Comet.WPF.Handlers;
using WGrid = System.Windows.Controls.Grid;

namespace Comet.WPF
{
    public class CometContainerView : WGrid
    {
        private View _view;
        private UIElement _nativeView;
        private IViewHandler _handler;

        public CometContainerView(View view = null)
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
                SetRow(_nativeView, 0);
                SetColumn(_nativeView, 0);
                SetColumnSpan(_nativeView, 1);
                SetRowSpan(_nativeView, 1);

                Children.Add(_nativeView);
            }
        }
    }
}

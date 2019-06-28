using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

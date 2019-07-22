using HotUI.WPF.Handlers;
using System.Windows;
using System.Windows.Controls;
using WGrid = System.Windows.Controls.Grid;
using WPFSize = System.Windows.Size;

namespace HotUI.WPF
{
    public class HUIListCell : WGrid
    {
        private View _view;
        private UIElement _nativeView;
        private IViewHandler _handler;

        public HUIListCell(View view = null)
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
                if (_nativeView is FrameworkElement frameworkElement)
                {
                    WGrid.SetRow(frameworkElement, 0);
                    WGrid.SetColumn(frameworkElement, 0);
                    WGrid.SetColumnSpan(frameworkElement, 1);
                    WGrid.SetRowSpan(frameworkElement, 1);
                }

                Children.Add(_nativeView);
            }
        }

        protected override WPFSize MeasureOverride(WPFSize availableSize)
        {
            // todo: this is a hack for now to avoid an infinite width
            if (double.IsInfinity(availableSize.Width))
                availableSize.Width = 800;

            var measuredSize = _view?.Measure(availableSize.ToSizeF()).ToSize();
            return measuredSize ?? availableSize;
        }

        protected override WPFSize ArrangeOverride(WPFSize finalSize)
        {
            if (finalSize.Width > 0 && finalSize.Height > 0 && _view != null)
                _view.Frame = new RectangleF(0, 0, (float)finalSize.Width, (float)finalSize.Height);

            return finalSize;
        }
    }
}

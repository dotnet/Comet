using System.Windows;
using System.Windows.Controls;
using WGrid = System.Windows.Controls.Grid;

namespace Comet.WPF.Controls
{
    public class CUIContainerView : WGrid
    {
        private FrameworkElement _mainView;

        public UIElement MainView
        {
            get => _mainView;
            set
            {
                if (_mainView == value)
                    return;

                if (_mainView != null)
                {
                    Children.Remove(_mainView);
                }

                _mainView = value as FrameworkElement;

                if (_mainView != null)
                {
                    WGrid.SetRow(_mainView, 0);
                    WGrid.SetColumn(_mainView, 0);
                    WGrid.SetRowSpan(_mainView, 1);
                    WGrid.SetColumnSpan(_mainView, 1);
                    Children.Add(_mainView);
                }
            }
        }
    }
}

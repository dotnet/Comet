using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HotUI.UWP.Controls
{
    public class HUIContainerView : Grid
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
                    Grid.SetRow(_mainView, 0);
                    Grid.SetColumn(_mainView, 0);
                    Grid.SetRowSpan(_mainView, 1);
                    Grid.SetColumnSpan(_mainView, 1);
                    Children.Add(_mainView);
                }
            }
        }
    }
}

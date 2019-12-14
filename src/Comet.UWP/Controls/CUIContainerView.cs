using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WGrid = Windows.UI.Xaml.Controls.Grid;

namespace Comet.UWP.Controls
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
					SetRow(_mainView, 0);
					SetColumn(_mainView, 0);
					SetRowSpan(_mainView, 1);
					SetColumnSpan(_mainView, 1);
					Children.Add(_mainView);
				}
			}
		}
	}
}

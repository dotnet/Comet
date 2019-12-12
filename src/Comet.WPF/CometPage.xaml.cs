using System.Windows.Controls;
using WGrid = System.Windows.Controls.Grid;

namespace Comet.WPF
{
	public sealed partial class CometPage : Page
	{
		private View _view;
		private readonly Frame _frame;

		public CometPage(Frame frame, View view)
		{
			InitializeComponent();

			_frame = frame;
			View = view;
		}

		public View View
		{
			get => _view;
			private set
			{
				_view = value;
				Content = value?.ToEmbeddableView() ?? new WGrid();

				if (_view?.BuiltView is NavigationView nav)
				{
					nav.SetPerformNavigate(toView => {
						_frame.NavigationService.Navigate(new CometPage(_frame, toView));
					});
					nav.SetPerformPop(() => _frame.NavigationService.GoBack());				}
			}
		}
	}
}

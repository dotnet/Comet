using System.Drawing;
using System.Windows.Controls;
using WGrid = System.Windows.Controls.Grid;

namespace Comet.WPF
{
	public sealed partial class CometPage : Page, IViewHandler
	{
		private View _view;
		private readonly Frame _frame;

		public CometPage(Frame frame, View view)
		{
			InitializeComponent();

			_frame = frame;
			View = view;
		}

		CometContainerView CometView;
		public View View
		{
			get => _view;
			private set
			{
				_view = value;
				Content = CometView = (CometContainerView)value?.ToEmbeddableView() ?? new CometContainerView();

				if (_view?.BuiltView is NavigationView nav)
				{
					nav.SetPerformNavigate(toView => {
						_frame.NavigationService.Navigate(new CometPage(_frame, toView));
					});
					nav.SetPerformPop(() => _frame.NavigationService.GoBack());
				}
				if (_view.ViewHandler == null)
					_view.ViewHandler = this;
			}
		}

		public object NativeView => null;

		public bool HasContainer { get; set; }

		public void SetView(View view) => CometView.View = view;

		public void UpdateValue(string property, object value)
		{

		}

		public void Remove(View view)
		{

		}

		public SizeF GetIntrinsicSize(SizeF availableSize) => availableSize;

		public void SetFrame(RectangleF frame)
		{

		}
	}
}

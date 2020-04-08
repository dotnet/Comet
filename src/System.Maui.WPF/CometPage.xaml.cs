using System.Drawing;
using System.Windows.Controls;
using WGrid = System.Windows.Controls.Grid;

namespace System.Maui.WPF
{
	public sealed partial class System.MauiPage : Page, IViewHandler
	{
		private View _view;
		private readonly Frame _frame;

		public System.MauiPage(Frame frame, View view)
		{
			InitializeComponent();

			_frame = frame;
			View = view;
		}

		System.MauiContainerView System.MauiView;
		public View View
		{
			get => _view;
			private set
			{
				_view = value;
				Content = System.MauiView = (System.MauiContainerView)value?.ToEmbeddableView() ?? new System.MauiContainerView();

				if (_view?.BuiltView is NavigationView nav)
				{
					nav.SetPerformNavigate(toView => {
						_frame.NavigationService.Navigate(new System.MauiPage(_frame, toView));
					});
					nav.SetPerformPop(() => _frame.NavigationService.GoBack());
				}
				if (_view.ViewHandler == null)
					_view.ViewHandler = this;
			}
		}

		public object NativeView => null;

		public bool HasContainer { get; set; }

		public void SetView(View view) => System.MauiView.View = view;

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

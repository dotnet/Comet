using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WGrid = Windows.UI.Xaml.Controls.Grid;

namespace Comet.UWP
{
	public sealed partial class CometPage : Page, IViewHandler
	{
		private View _view;
		private static bool _initializedBack;

		public CometPage()
		{
			Init();
		}

		public CometPage(View view)
		{
			Init();
			View = view;
		}
		void Init()
		{
			InitializeComponent();
			NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
		}
		CometView CometView;

		private View View
		{
			get => _view;
			set
			{
				if (_view != null)
				{
					_view.ViewHandler = null;
				}

				_view = value;
				Content = CometView =  (CometView)_view.ToEmbeddableView();
				if (_view?.BuiltView is NavigationView nav)
				{
					if (!_initializedBack)
					{
						SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
						_initializedBack = true;
					}

					nav.SetPerformNavigate(toView => {
						SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
						Frame.Navigate(typeof(CometPage), toView);
						DiscardHandlers(_view);
					});
					nav.SetPerformPop(() => {
						Frame.GoBack();
					});
				}
				if (_view.ViewHandler == null)
					_view.ViewHandler = this;


			}
		}

		public object NativeView => null;

		public bool HasContainer { get; set; }

		private void DiscardHandlers(View view)
		{
			if (view == null)
				return;

			view.ViewHandler = null;
			if (view.BuiltView != null && view.BuiltView != view)
				DiscardHandlers(view.BuiltView);

			if (view is NavigationView navigationView)
			{
				DiscardHandlers(navigationView.Content);
			}
			if (view is IEnumerable<View> views && (views.GetEnumerator() != null))
			{
				foreach (var subview in views)
				{
					DiscardHandlers(subview);
				}
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			View = e.Parameter as View;
			base.OnNavigatedTo(e);
		}

		private static void OnBackRequested(object sender, BackRequestedEventArgs e)
		{
			var rootFrame = Window.Current.Content as Frame;

			if (rootFrame?.CanGoBack ?? false)
			{
				e.Handled = true;
				rootFrame?.GoBack();

				if (rootFrame?.CanGoBack ?? false)
					SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
				else
					SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
			}
		}

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

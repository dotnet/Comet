using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace HotUI.UWP
{
    public sealed partial class HotUIPage : Page
    {
        private View _view;
        private static bool _initializedBack;

        public HotUIPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        public HotUIPage(View view)
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
            View = view;
        }

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
                Content = value?.ToEmbeddableView() ?? new Grid();

                if (_view?.BuiltView is NavigationView nav)
                {
                    if (!_initializedBack)
                    {
                        SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
                        _initializedBack = true;
                    }

                    nav.PerformNavigate = toView =>
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                        this.Frame.Navigate(typeof(HotUIPage), toView);
                        DiscardHandlers(_view);
                    };
                }
            }
        }

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
            if (view is IEnumerable<View> views)
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
            Frame rootFrame = Window.Current.Content as Frame;

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
    }
}

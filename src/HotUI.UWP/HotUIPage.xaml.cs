using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HotUI.UWP
{
    public sealed partial class HotUIPage : Page
    {
        private View _view;

        public HotUIPage()
        {
            this.InitializeComponent();
            View = null;
        }

        public HotUIPage(View view)
        {
            this.InitializeComponent();
            View = view;
        }

        private View View
        {
            get => _view;
            set
            {
                _view = value;
                Content = value?.ToEmbeddableView() ?? new Grid();

                if (_view?.BuiltView is NavigationView nav)
                {
                    nav.PerformNavigate = toView =>
                    {
                        this.Frame.Navigate(typeof(HotUIPage), toView);
                    };
                }
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            View = e.Parameter as View;
            base.OnNavigatedTo(e);
        }
    }
}

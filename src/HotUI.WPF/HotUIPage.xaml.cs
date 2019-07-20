using System.Windows.Controls;
using WGrid = System.Windows.Controls.Grid;

namespace HotUI.WPF
{
    public sealed partial class HotUIPage : Page
    {
        private View _view;
        private readonly Frame _frame;

        public HotUIPage(Frame frame, View view)
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
                    nav.PerformNavigate = toView =>
                    {
                        _frame.NavigationService.Navigate(new HotUIPage(_frame, toView));
                    };
                }
            }
        }
    }
}

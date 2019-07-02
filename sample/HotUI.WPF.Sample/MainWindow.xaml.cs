using HotUI.Samples;
using System.Windows;

namespace HotUI.WPF.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.NavigationService.Navigate(new HotUIPage(MainFrame, new BasicNavigationTestView()));
        }
    }
}

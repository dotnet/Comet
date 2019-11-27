using Comet.Samples;
using System.Windows;

namespace Comet.WPF.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Comet.Skia.UI.Init();
            //Comet.Skia.Controls.Init();

            MainFrame.NavigationService.Navigate(new CometPage(MainFrame, new MainPage()));
        }
    }
}

using System.Maui.Samples;
using System.Maui.Styles.Material;
using System.Windows;

namespace System.Maui.WPF.Sample
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			new MaterialStyle(ColorPalette.Blue).Apply();
			System.Maui.Skia.UI.Init();
			//System.Maui.Skia.Controls.Init();

			MainFrame.NavigationService.Navigate(new System.MauiPage(MainFrame, new MainPage()));
		}
	}
}

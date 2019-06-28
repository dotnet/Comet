using HotUI.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var hotuiView = new BasicTestView().ToEmbeddableView();
            MainView.Children.Add(hotuiView);
        }
    }
}

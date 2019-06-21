using System;
using HotUI.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HotForms.Sample {
	public partial class App : Application {
		public App ()
		{
			InitializeComponent ();
			HotUI.Forms.UI.Init ();
			MainPage = new NavigationPage (new MyDynamicStatePage ().ToForms());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

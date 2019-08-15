using System;
using Comet.Samples;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Comet.Forms.Sample {
	public partial class App : Application {
		public App ()
		{
			InitializeComponent ();

			MainPage = new MainPage ().ToPage ();
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

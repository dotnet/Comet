using Android.App;
using Android.Runtime;
using Comet.Samples;

namespace Comet.Sample
{
	[Application]
	public class MainApplication : MauiApplication
	{
		public MainApplication(IntPtr handle, JniHandleOwnership ownership)
			: base(handle, ownership)
		{
		}

		protected override MauiApp CreateMauiApp() => MyApp.CreateMauiApp();
	}
}

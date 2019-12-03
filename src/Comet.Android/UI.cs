using Comet.Android.Controls;
using Comet.Android.Handlers;
using Comet.Android.Services;

namespace Comet.Android
{
	public static class UI
	{
		static bool _hasInitialized;

		public static void Init()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;

			// Controls
			Registrar.Handlers.Register<Button, ButtonHandler>();
			Registrar.Handlers.Register<Image, ImageHandler>();
			Registrar.Handlers.Register<ProgressBar, ProgressBarHandler>();
			Registrar.Handlers.Register<Slider, SliderHandler>();
			Registrar.Handlers.Register<TextField, TextFieldHandler>();
			Registrar.Handlers.Register<Text, TextHandler>();
			Registrar.Handlers.Register<Toggle, ToggleHandler>();
			Registrar.Handlers.Register<ActivityIndicator, ActivityIndicatorHandler>();
			//Registrar.Handlers.Register<WebView, WebViewHandler> ();

			// Containers
			Registrar.Handlers.Register<ScrollView, ScrollViewHandler>();
			Registrar.Handlers.Register<ListView, ListViewHandler>();
			Registrar.Handlers.Register<View, ViewHandler>();
			Registrar.Handlers.Register<ContentView, ContentViewHandler>();
			Registrar.Handlers.Register<ViewRepresentable, ViewRepresentableHandler>();
			Registrar.Handlers.Register<TabView, TabViewHandler>();
			Registrar.Handlers.Register<NavigationView, NavigationViewHandler>();

			// Native Layouts
			Registrar.Handlers.Register<HStack, HStackHandler>();
			Registrar.Handlers.Register<VStack, VStackHandler>();

			// Managed Layouts
			//Registrar.Handlers.Register<HStack, ManagedHStackHandler>();
			//Registrar.Handlers.Register<VStack, ManagedVStackHandler>();
			//Registrar.Handlers.Register<ZStack, ManagedZStackHandler>();
			//Registrar.Handlers.Register<Grid, ManagedGridHandler>();

			// Modal SUpport
			ModalView.PerformPresent = ModalManager.ShowModal;
			ModalView.PerformDismiss = ModalManager.DismisModal;

			// Device Services
			ThreadHelper.JoinableTaskContext = new Microsoft.VisualStudio.Threading.JoinableTaskContext();
			Device.GraphicsService = new AndroidGraphicsService();
			Device.BitmapService = new AndroidBitmapService();


			AnimationManger.SetTicker(new AndroidTicker());
		}
	}
}

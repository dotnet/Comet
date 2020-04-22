using System.Maui.Android.Controls;
using System.Maui.Android.Handlers;
using System.Maui.Android.Services;
using System.Maui.Styles;

namespace System.Maui.Android
{
	public static class UI
	{
		static bool _hasInitialized;

		public static void Init()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;

			// Controls
			Registrar.Handlers.Register<ActivityIndicator, ActivityIndicatorHandler>();
			Registrar.Handlers.Register<Button, ButtonHandler>();
			Registrar.Handlers.Register<Image, ImageHandler>();
			Registrar.Handlers.Register<ProgressBar, ProgressBarHandler>();
			Registrar.Handlers.Register<SecureField, SecureFieldHandler>();
			Registrar.Handlers.Register<Slider, SliderHandler>();
			Registrar.Handlers.Register<RadioButton, RadioButtonHandler>();
			// Stepper
			Registrar.Handlers.Register<Label, TextHandler>();
			Registrar.Handlers.Register<Entry, TextFieldHandler>();
			Registrar.Handlers.Register<Switch, ToggleHandler>();
			//Registrar.Handlers.Register<WebView, WebViewHandler> ();

			// Containers
			Registrar.Handlers.Register<ContentView, ContentViewHandler>();
			Registrar.Handlers.Register<ListView, ListViewHandler>();
			Registrar.Handlers.Register<ScrollView, ScrollViewHandler>();
			Registrar.Handlers.Register<View, ViewHandler>();
			Registrar.Handlers.Register<ViewRepresentable, ViewRepresentableHandler>();
			Registrar.Handlers.Register<TabView, TabViewHandler>();
			Registrar.Handlers.Register<NavigationView, NavigationViewHandler>();
			Registrar.Handlers.Register<RadioGroup, RadioGroupHandler>();

			// Layouts
			Registrar.Handlers.Register<HStack, HStackHandler>();
			Registrar.Handlers.Register<VStack, VStackHandler>();
			Registrar.Handlers.Register<ZStack, ZStackHandler>();
			Registrar.Handlers.Register<Grid, GridHandler>();
			Registrar.Handlers.Register<Spacer, SpacerHandler>();

			// Modal Support
			ModalView.PerformPresent = ModalManager.ShowModal;
			ModalView.PerformDismiss = ModalManager.DismisModal;

			// Device Services
			ThreadHelper.JoinableTaskContext = new Microsoft.VisualStudio.Threading.JoinableTaskContext();
			Device.GraphicsService = new AndroidGraphicsService();
			Device.BitmapService = new AndroidBitmapService();


			AnimationManger.SetTicker(new AndroidTicker());

			//Set Default Style
			var style = new Style();
			style.Apply();

		}
	}
}

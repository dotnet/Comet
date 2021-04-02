using System;
//using Xamarin.Platform;
//using Xamarin.Platform.Handlers;
//using RegistrarHandlers = Xamarin.Platform.Registrar;
namespace Comet
{
	public static class CometPlatform
	{
#if __IOS__

		static Foundation.NSObject _invoker = new Foundation.NSObject();
#endif
		static bool HasInit = false;
		public static void Init()
		{
			if (HasInit)
				return;
			//RegistrarHandlers.Handlers.Register<Button, ButtonHandler>();
			//RegistrarHandlers.Handlers.Register<Text, LabelHandler>();
			//RegistrarHandlers.Handlers.Register<Slider, SliderHandler>();
			//RegistrarHandlers.Handlers.Register<VStack, LayoutHandler>();
			//RegistrarHandlers.Handlers.Register<HStack, LayoutHandler>();
			//RegistrarHandlers.Handlers.Register<ZStack, LayoutHandler>();
			//RegistrarHandlers.Handlers.Register<Grid, LayoutHandler>();
#if __IOS__
			// Device Features
			//ModalView.PerformPresent = (o) => {
			//	PresentingViewController.PresentViewController(o.ToViewController(), true, null);
			//};
			//ModalView.PerformDismiss = () => PresentingViewController.DismissModalViewController(true);
			
			ThreadHelper.Setup(_invoker.BeginInvokeOnMainThread);

			//Device.FontService = new iOSFontService();
			//Device.GraphicsService = new iOSGraphicsService();
			//Device.BitmapService = new iOSBitmapService();

			//AnimationManger.SetTicker(new iOSTicker());

			//Set Default Style
			var style = new Styles.Style();
			style.Apply();
#endif
		}

		public static void Reinit()
		{
			HasInit = false;
			Init();
		}
	}
}

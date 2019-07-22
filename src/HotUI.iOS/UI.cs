
using System;
using Foundation;
using HotUI;
using HotUI.iOS.Handlers;
using HotUI.iOS.Services;
using UIKit;

namespace HotUI.iOS
{
	public static class UI
    {
		static bool _hasInitialized;
		static NSObject _invoker = new NSObject ();

		public static void Init ()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;

            // Controls
            Registrar.Handlers.Register<Button, ButtonHandler> ();
            Registrar.Handlers.Register<Image, ImageHandler>();
            Registrar.Handlers.Register<TextField, TextFieldHandler> ();
			Registrar.Handlers.Register<Text, TextHandler> ();
            Registrar.Handlers.Register<SecureField, SecureFieldHandler>();
            Registrar.Handlers.Register<Slider, SliderHandler>();
            Registrar.Handlers.Register<ShapeView, ShapeViewHandler>();
            Registrar.Handlers.Register<Toggle, ToggleHandler>();
            //Registrar.Handlers.Register<WebView, WebViewHandler> ();

            // Containers
            Registrar.Handlers.Register<ContentView, ContentViewHandler>();
            Registrar.Handlers.Register<ListView, ListViewHandler>();
            Registrar.Handlers.Register<ScrollView, ScrollViewHandler> ();
			Registrar.Handlers.Register<View, ViewHandler> ();
            Registrar.Handlers.Register<ViewRepresentable, ViewRepresentableHandler>();
            Registrar.Handlers.Register<TabView, TabViewHandler>();

            // Managed Layout
            Registrar.Handlers.Register<HStack, HStackHandler>();
            Registrar.Handlers.Register<VStack, VStackHandler>();
            Registrar.Handlers.Register<ZStack, ZStackHandler>();
            Registrar.Handlers.Register<Grid, GridHandler>();
            Registrar.Handlers.Register<Spacer, SpacerHandler>();

            // Device Features
            ModalView.PerformPresent = (o) => {
				PresentingViewController.PresentViewController (o.ToViewController(), true,null);
			};
			ModalView.PerformDismiss = () => PresentingViewController.DismissModalViewController (true);

            // Device Features
            Device.PerformInvokeOnMainThread = _invoker.BeginInvokeOnMainThread;
			Device.FontService = new iOSFontService();
			Device.GraphicsService = new iOSGraphicsService();
			Device.BitmapService = new iOSBitmapService();
		}

		internal static UIViewController PresentingViewController {
			get {
				//if (overrideVc != null)
				//    return overrideVc;

				var window = UIApplication.SharedApplication.KeyWindow;
				var vc = window.RootViewController;
				while (vc.PresentedViewController != null)
					vc = vc.PresentedViewController;
				return vc;
			}
		}
	}
}

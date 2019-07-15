
using System;
using Foundation;
using HotUI;
using HotUI.iOS.Services;
using UIKit;

namespace HotUI.iOS {
	public static class UI {
		static bool hasInit;
		static NSObject invoker = new NSObject ();
		public static void Init ()
		{
			if (hasInit)
				return;
			hasInit = true;
			Registrar.Handlers.Register<Button, ButtonHandler> ();
			Registrar.Handlers.Register<TextField, TextFieldHandler> ();
			Registrar.Handlers.Register<SecureField, SecureFieldHandler>();
			Registrar.Handlers.Register<Text, TextHandler> ();
            Registrar.Handlers.Register<Toggle, ToggleHandler>();
			Registrar.Handlers.Register<VStack, VStackHandler> ();
			Registrar.Handlers.Register<HStack, HStackHandler> ();
			//Registrar.Handlers.Register<WebView, WebViewHandler> ();
			Registrar.Handlers.Register<ScrollView, ScrollViewHandler> ();
			Registrar.Handlers.Register<Image, ImageHandler> ();
			Registrar.Handlers.Register<ListView, ListViewHandler> ();
			Registrar.Handlers.Register<View, ViewHandler> ();
			Registrar.Handlers.Register<ContentView, ContentViewHandler> ();
            Registrar.Handlers.Register<Spacer, SpacerHandler>();
            Registrar.Handlers.Register<ShapeView, ShapeViewHandler>();
            Registrar.Handlers.Register<ViewRepresentable, ViewRepresentableHandler>();
            Registrar.Handlers.Register<Slider, SliderHandler>();

            ModalView.PerformPresent = (o) => {
				PresentingViewController.PresentViewController (o.ToViewController(), true,null);
			};
			ModalView.PerformDismiss = () => PresentingViewController.DismissModalViewController (true);
			Device.PerformInvokeOnMainThread = invoker.BeginInvokeOnMainThread;
			Device.FontService = new iOSFontService();
			Device.GraphicsService = new iOSGraphicsService();
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

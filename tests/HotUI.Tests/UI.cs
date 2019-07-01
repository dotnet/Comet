using System;
using System.Threading;

namespace HotUI.Tests {
	public static class UI {
		static bool hasInit;
		public static void Init ()
		{
			if (hasInit)
				return;
			hasInit = true;
			Registrar.Handlers.Register<Button, GenericViewHandler> ();
			Registrar.Handlers.Register<TextField, GenericViewHandler> ();
			Registrar.Handlers.Register<Text, GenericViewHandler> ();
			Registrar.Handlers.Register<Toggle, GenericViewHandler> ();
			Registrar.Handlers.Register<VStack, GenericViewHandler> ();
			Registrar.Handlers.Register<HStack, GenericViewHandler> ();
			Registrar.Handlers.Register<ScrollView, GenericViewHandler> ();
			Registrar.Handlers.Register<Image, GenericViewHandler> ();
			Registrar.Handlers.Register<ListView, GenericViewHandler> ();
			Registrar.Handlers.Register<View, GenericViewHandler> ();
			Registrar.Handlers.Register<ContentView, GenericViewHandler> ();


			Device.PerformInvokeOnMainThread = (a) => a ();
		}
	}
}

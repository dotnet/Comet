using System.Maui.Blazor.Handlers;

namespace System.Maui.Blazor
{
	internal static class UI
	{
		private static bool _hasInitialized;

		public static void Init()
		{
			if (_hasInitialized)
			{
				return;
			}

			_hasInitialized = true;

			Registrar.Handlers.Register<Text, TextHandler>();
			Registrar.Handlers.Register<ContainerView, ContainerViewHandler>();
			Registrar.Handlers.Register<Button, ButtonHandler>();
			Registrar.Handlers.Register<ContentView, ContentViewHandler>();
			Registrar.Handlers.Register<View, ViewHandler>();
			Registrar.Handlers.Register<ListView, ListViewHandler>();
			Registrar.Handlers.Register<Spacer, SpacerHandler>();
			Registrar.Handlers.Register<TextField, TextFieldHandler>();
			Registrar.Handlers.Register<ProgressBar, ProgressBarHandler>();
			Registrar.Handlers.Register<Image, ImageHandler>();
			Registrar.Handlers.Register<ScrollView, ScrollViewHandler>();
			Registrar.Handlers.Register<TabView, TabViewHandler>();
			Registrar.Handlers.Register<ShapeView, ShapeViewHandler>();

			// Unsupported views. Without registering these, it cause an infinite recursion on derived views
			Registrar.Handlers.Register<ActivityIndicator, UnsupportedHandler<ActivityIndicator>>();
			Registrar.Handlers.Register<SecureField, UnsupportedHandler<SecureField>>();
			Registrar.Handlers.Register<Slider, UnsupportedHandler<Slider>>();
			Registrar.Handlers.Register<Toggle, UnsupportedHandler<Toggle>>();
			Registrar.Handlers.Register<DrawableControl, UnsupportedHandler<DrawableControl>>();
			Registrar.Handlers.Register<ViewRepresentable, UnsupportedHandler<ViewRepresentable>>();
			Registrar.Handlers.Register<WebView, UnsupportedHandler<WebView>>();

			ThreadHelper.JoinableTaskContext = new Microsoft.VisualStudio.Threading.JoinableTaskContext();
			ListView.HandlerSupportsVirtualization = false;
		}
	}
}

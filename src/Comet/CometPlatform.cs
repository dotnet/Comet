using System;
using Comet.Handlers;
using Xamarin.Platform.Handlers;
using registrar = Xamarin.Platform.Registrar;
namespace Comet
{
	public partial class CometPlatform
	{
		static bool HasInit;

		public static void Init()
		{
			if (HasInit)
				return;

			HasInit = true;
			registrar.Handlers.Register<View, CometViewHandler> ();
			registrar.Handlers.Register<AbstractLayout, LayoutHandler>();
			registrar.Handlers.Register<Button, ButtonHandler>();
			registrar.Handlers.Register<Text, LabelHandler>();


			ThreadHelper.JoinableTaskContext = new Microsoft.VisualStudio.Threading.JoinableTaskContext();
			nativeInit();
			
		}
		static partial void nativeInit();
	}
}

using System;
using Xamarin.Platform.Handlers;
using registrar = Xamarin.Platform.Registrar;
namespace Comet
{
	public class CometPlatform
	{
		static bool HasInit;

		public static void Init()
		{
			if (HasInit)
				return;

			HasInit = true;

			registrar.Handlers.Register<AbstractLayout, LayoutHandler>();
			registrar.Handlers.Register<Button, ButtonHandler>();
			registrar.Handlers.Register<Text, LabelHandler>();
		}
	}
}

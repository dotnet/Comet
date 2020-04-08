using System.Maui.Blazor.Handlers;

namespace System.Maui.Blazor
{
	internal static class BlazorExtensions
	{
		public static IBlazorViewHandler GetOrCreateViewHandler(this View view)
		{
			if (view == null)
			{
				return null;
			}

			var handler = view.ViewHandler;
			if (handler == null)
			{
				handler = Registrar.Handlers.GetHandler(view.GetType());
				view.ViewHandler = handler;
			}

			return handler as IBlazorViewHandler;
		}
	}
}

using System.Maui.Blazor.Components;

namespace System.Maui.Blazor.Handlers
{
	internal class ContentViewHandler : BlazorHandler<ContentView, BView>
	{
		public static readonly PropertyMapper<ContentView> Mapper = new PropertyMapper<ContentView>
		{
			{ nameof(ContentView.Content), MapContentProperty }
		};

		public ContentViewHandler()
			: base(Mapper)
		{
		}

		public static void MapContentProperty(IViewHandler viewHandler, ContentView virtualView)
		{
			var nativeView = (BView)viewHandler.NativeView;

			nativeView.View = virtualView.Content;
		}
	}
}

using System.Maui.Blazor.Components;

namespace System.Maui.Blazor.Handlers
{
	internal class ScrollViewHandler : BlazorHandler<ScrollView, BView>
	{
		public static readonly PropertyMapper<ScrollView> Mapper = new PropertyMapper<ScrollView>
		{
			{ nameof(ScrollView.View), MapContentProperty }
		};

		public ScrollViewHandler()
			: base(Mapper)
		{
		}

		public static void MapContentProperty(IViewHandler viewHandler, ScrollView virtualView)
		{
			var nativeView = (BView)viewHandler.NativeView;

			nativeView.View = virtualView.View;
		}
	}
}

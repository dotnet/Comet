using System.Maui.Blazor.Components;

namespace System.Maui.Blazor.Handlers
{
	internal class ListViewHandler : BlazorHandler<ListView, BListView>
	{
		public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>
		{
			{ "List", MapListProperty },
			{ nameof(ListView.ItemSelected), MapItemSelectedProperty },
		};

		public ListViewHandler()
			: base(Mapper)
		{
		}

		public static void MapListProperty(IViewHandler viewHandler, ListView virtualView)
		{
			var nativeView = (BListView)viewHandler.NativeView;

			nativeView.List = virtualView;
		}

		public static void MapItemSelectedProperty(IViewHandler viewHandler, ListView virtualView)
		{
			var nativeView = (BListView)viewHandler.NativeView;

			nativeView.HasOnSelected = virtualView.ItemSelected != null;
		}
	}
}

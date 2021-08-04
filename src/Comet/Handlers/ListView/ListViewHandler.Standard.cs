using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class ListViewHandler : ViewHandler<IListView, object>
	{
		public static void MapListViewProperty(IElementHandler viewHandler, IListView virtualView)
		{
		}

		public static void MapReloadData(IElementHandler viewHandler, IListView virtualView, object? value)
		{
		}

		protected override object CreateNativeView() => throw new NotImplementedException();
	}
}

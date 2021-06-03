using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class ListViewHandler : ViewHandler<IListView, object>
	{
		public static void MapListViewProperty(IViewHandler viewHandler, IListView virtualView)
		{
		}

		public static void MapReloadData(IViewHandler viewHandler, IListView virtualView)
		{
		}

		protected override object CreateNativeView() => throw new NotImplementedException();
	}
}

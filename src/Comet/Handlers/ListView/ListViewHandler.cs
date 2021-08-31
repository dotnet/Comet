using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class ListViewHandler 
	{
		public static readonly PropertyMapper<IListView, ListViewHandler> Mapper = new PropertyMapper<IListView, ListViewHandler>(ViewHandler.ViewMapper)
		{
			["ListView"] = MapListViewProperty,

		};
		public static readonly CommandMapper<IListView, ListViewHandler> ActionMapper = new CommandMapper<IListView, ListViewHandler>
		{
			[nameof(ListView.ReloadData)] = MapReloadData,
		};

		public ListViewHandler() : base(Mapper, ActionMapper)
		{

		}
		public ListViewHandler(PropertyMapper<IListView> mapper) : base(mapper)
		{

		}
	}
}

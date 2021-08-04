using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class ListViewHandler 
	{
		public static readonly PropertyMapper<IListView> Mapper = new PropertyMapper<IListView>(ViewHandler.ViewMapper)
		{
			["ListView"] = MapListViewProperty,

		};
		public static readonly CommandMapper<IListView> ActionMapper = new CommandMapper<IListView>
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

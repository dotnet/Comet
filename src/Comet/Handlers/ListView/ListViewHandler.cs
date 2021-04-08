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
			[nameof(ListView.ReloadData)] = MapReloadData,

		};

		public ListViewHandler() : base(Mapper)
		{

		}
		public ListViewHandler(PropertyMapper<IListView> mapper) : base(mapper)
		{

		}
	}
}

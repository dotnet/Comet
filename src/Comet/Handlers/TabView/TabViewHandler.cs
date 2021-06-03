using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class TabViewHandler
	{
		public static readonly PropertyMapper<TabView, TabViewHandler> Mapper = new PropertyMapper<TabView, TabViewHandler>(ViewHandler.ViewMapper)
		{
			[nameof(IContainer.Children)] = MapChildren

		};

		public TabViewHandler() : base(Mapper)
		{

		}
		public TabViewHandler(PropertyMapper<TabView> mapper) : base(mapper)
		{

		}
	}
}

using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class TabViewHandler
	{

		public TabViewHandler() : base(ViewHandler.ViewMapper)
		{

		}
		public TabViewHandler(PropertyMapper<TabView> mapper) : base(mapper)
		{

		}
	}
}

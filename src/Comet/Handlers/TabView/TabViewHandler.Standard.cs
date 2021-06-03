using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class TabViewHandler : ViewHandler<TabView, object>
	{
		public static void MapChildren(TabViewHandler handler, TabView tabView) 
		{
		}
		protected override object CreateNativeView() => throw new NotImplementedException();
	}
}

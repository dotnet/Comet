using System;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace Comet.Handlers
{
	public partial class TabViewHandler : ViewHandler<TabView, Panel>
	{
		//public override bool IgnoreSafeArea => VirtualView?.GetIgnoreSafeArea(true) ?? true;
		protected override Panel CreateNativeView() => NativeView ?? new LayoutPanel { };


		public static void MapChildren(TabViewHandler handler, TabView tabView)
		{
			//handler?.NativeView?.Setup(tabView?.ToList());
		}
	}
}

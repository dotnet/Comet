using System;
using System.Linq;
using Comet.iOS;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class TabViewHandler : ViewHandler<TabView, CUITabView>
	{
		//public override bool IgnoreSafeArea => VirtualView?.GetIgnoreSafeArea(true) ?? true;
		protected override CUITabView CreateNativeView() => NativeView ?? new CUITabView { Context = MauiContext };


		public static void MapChildren(TabViewHandler handler, TabView tabView) => handler?.NativeView?.Setup(tabView?.ToList());
	}
}

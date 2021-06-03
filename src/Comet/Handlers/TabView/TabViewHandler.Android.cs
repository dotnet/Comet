using System;
using Android.Views;
using Android.Widget;
using AView = Android.Views.View;
using Comet.Android.Controls;
using Android.Content;
using Android.Util;
using System.Linq;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class TabViewHandler : ViewHandler<TabView, CometTabView>
	{
		//private AView _view;
		protected override CometTabView CreateNativeView() => new CometTabView(MauiContext);

		public static void MapChildren(TabViewHandler handler, TabView tabView) => handler?.NativeView?.CreateTabs(tabView?.ToList());
	}
}

using System;
using Android.Views;
using Android.Widget;
using AView = Android.Views.View;
using Comet.Android.Controls;
using Android.Content;
using Android.Util;
using System.Linq;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;

namespace Comet.Handlers
{
	public partial class TabViewHandler : ViewHandler<TabView, CometTabView>
	{
		//private AView _view;
		protected override CometTabView CreateNativeView() => new CometTabView(MauiContext);


		protected override void ConnectHandler(CometTabView nativeView)
		{
			base.ConnectHandler(nativeView);
		}
		public override void SetVirtualView(IView view) {
			base.SetVirtualView(view);

			NativeView?.CreateTabs(this.VirtualView);
		}
	}
}

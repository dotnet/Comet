using System;
using Comet.Android.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
//using Comet.iOS;
namespace Comet.Handlers
{
	public partial class NavigationViewHandler : ViewHandler<NavigationView, CometNavigationView>, IPlatformViewHandler
	{
		protected override CometNavigationView CreatePlatformView()
			=> new CometNavigationView(MauiContext);
		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);
			if (VirtualView != null)
			{
				PlatformView.SetRoot(VirtualView.Content);
				VirtualView?.SetPerformNavigate(PlatformView.NavigateTo);
				VirtualView?.SetPerformPop(PlatformView.Pop);
			}
		}
		protected override void DisconnectHandler(CometNavigationView nativeView)
		{
			base.DisconnectHandler(nativeView);

			if (VirtualView != null)
			{
				VirtualView.SetPerformNavigate(action: null);
				VirtualView.SetPerformPop(action: null);
			}

		}
	}
}

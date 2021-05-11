using System;
using Comet.Android.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
//using Comet.iOS;
namespace Comet.Handlers
{
	public partial class NavigationViewHandler : ViewHandler<NavigationView, CometNavigationView>, INativeViewHandler
	{
		protected override CometNavigationView CreateNativeView()
			=> new CometNavigationView(MauiContext);
		public override void SetVirtualView(IView view)
		{
			if (view == VirtualView)
				return;
			base.SetVirtualView(view);
			if (VirtualView != null)
			{
				NativeView.SetRoot(VirtualView.Content);
				VirtualView?.SetPerformNavigate(NativeView.NavigateTo);
				VirtualView?.SetPerformPop(NativeView.Pop);
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

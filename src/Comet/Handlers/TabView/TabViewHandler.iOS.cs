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
		protected override CUITabView CreatePlatformView() =>  new CUITabView { Context = MauiContext };


		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			PlatformView?.Setup(this.VirtualView);
		}

	}
}

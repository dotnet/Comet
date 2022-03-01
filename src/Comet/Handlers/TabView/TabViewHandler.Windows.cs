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
		protected override Panel CreatePlatformView() => PlatformView ?? new LayoutPanel { };


		//public override void SetVirtualView(IView view)
		//{
		//	base.SetVirtualView(view);

		//	PlatformView?.Setup(this.VirtualView);
		//}
	}
}

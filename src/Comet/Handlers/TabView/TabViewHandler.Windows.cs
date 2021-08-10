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
		protected override Panel CreateNativeView() => NativeView ?? new LayoutPanel { };


		//public override void SetVirtualView(IView view)
		//{
		//	base.SetVirtualView(view);

		//	NativeView?.Setup(this.VirtualView);
		//}
	}
}

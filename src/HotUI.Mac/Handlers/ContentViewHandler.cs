using AppKit;
using HotUI.Mac.Extensions;

// ReSharper disable ClassNeverInstantiated.Global
namespace HotUI.Mac.Handlers 
{
	public class ContentViewHandler : AbstractHandler<ContentView, NSView>
	{
		protected override NSView CreateView()
		{
			return VirtualView?.Content.ToView ();
		}
	}
}

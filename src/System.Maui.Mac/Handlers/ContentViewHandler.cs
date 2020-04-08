using AppKit;
using System.Maui.Mac.Extensions;

// ReSharper disable ClassNeverInstantiated.Global
namespace System.Maui.Mac.Handlers
{
	public class ContentViewHandler : AbstractHandler<ContentView, NSView>
	{
		protected override NSView CreateView()
		{
			return VirtualView?.Content.ToView();
		}
	}
}

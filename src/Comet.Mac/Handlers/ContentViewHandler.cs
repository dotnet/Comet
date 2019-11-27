using AppKit;
using Comet.Mac.Extensions;

// ReSharper disable ClassNeverInstantiated.Global
namespace Comet.Mac.Handlers
{
	public class ContentViewHandler : AbstractHandler<ContentView, NSView>
	{
		protected override NSView CreateView()
		{
			return VirtualView?.Content.ToView();
		}
	}
}

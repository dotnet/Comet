using Windows.UI.Xaml;

namespace Comet.UWP.Handlers
{
	public class ContentViewHandler : AbstractHandler<ContentView, UIElement>
	{
		protected override UIElement CreateView()
		{
			return VirtualView?.Content.ToView();
		}
	}
}

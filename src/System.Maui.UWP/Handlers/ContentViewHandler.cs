using Windows.UI.Xaml;

namespace System.Maui.UWP.Handlers
{
	public class ContentViewHandler : AbstractHandler<ContentView, UIElement>
	{
		protected override UIElement CreateView()
		{
			return VirtualView?.Content.ToView();
		}
	}
}

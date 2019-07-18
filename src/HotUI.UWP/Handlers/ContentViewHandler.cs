using Windows.UI.Xaml;

namespace HotUI.UWP.Handlers
{
	public class ContentViewHandler : AbstractHandler<ContentView, UIElement>
    {
        protected override UIElement CreateView()
        {
            return VirtualView?.Content.ToView();
        }
    }
}

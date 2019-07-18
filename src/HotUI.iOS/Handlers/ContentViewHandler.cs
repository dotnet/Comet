using UIKit;

// ReSharper disable ClassNeverInstantiated.Global
namespace HotUI.iOS.Handlers
{
    public class ContentViewHandler : AbstractHandler<ContentView, UIView>
    {
        protected override UIView CreateView()
        {
            return VirtualView?.Content?.ToView();
        }
    }
}
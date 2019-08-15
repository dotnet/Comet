using FView = Xamarin.Forms.View;

namespace Comet.Forms.Handlers
{
    public class ContentViewHandler : AbstractHandler<ContentView, FView>
    {
        protected override FView CreateView()
        {
            return VirtualView?.Content?.ToForms();
        }
    }
}

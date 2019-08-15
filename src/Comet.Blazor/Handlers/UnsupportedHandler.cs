using Comet.Blazor.Components;

namespace Comet.Blazor.Handlers
{
    internal class UnsupportedHandler<TVirtualView> : BlazorHandler<TVirtualView, BUnsupported>
        where TVirtualView : View
    {
        protected override void NativeViewUpdated()
        {
            NativeView.View = VirtualView;
            base.NativeViewUpdated();
        }
    }
}

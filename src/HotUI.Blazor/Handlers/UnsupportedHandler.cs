using HotUI.Blazor.Components;

namespace HotUI.Blazor.Handlers
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

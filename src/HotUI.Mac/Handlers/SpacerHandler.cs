using AppKit;
using HotUI.Mac.Controls;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Mac.Handlers
{
    public class SpacerHandler : AbstractHandler<Spacer, NSColorView>
    {
        protected override NSColorView CreateView()
        {
            return new NSColorView();
        }
    }
}
using AppKit;
using Comet.Mac.Controls;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Mac.Handlers
{
    public class SpacerHandler : AbstractHandler<Spacer, NSColorView>
    {
        protected override NSColorView CreateView()
        {
            return new NSColorView();
        }
    }
}

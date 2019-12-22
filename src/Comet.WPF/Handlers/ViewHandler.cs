using System.Diagnostics;
using System.Windows;

// ReSharper disable MemberCanBePrivate.Global

namespace Comet.WPF.Handlers
{
    public class ViewHandler : AbstractHandler<View, UIElement>
    {

        protected override UIElement CreateView()
        {
            var viewHandler = VirtualView?.GetOrCreateViewHandler();
            if (viewHandler?.GetType() == typeof(ViewHandler) && VirtualView.Body == null)
            {
                Debug.WriteLine($"There is no ViewHandler for {VirtualView.GetType()}");
                return null;
            }

            return viewHandler?.View;
        }
    }
}

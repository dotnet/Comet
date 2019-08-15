using System;
using System.Linq;
using Android.Content;
using Comet.Android.Controls;

namespace Comet.Android.Handlers
{
    public class NavigationViewHandler : AbstractHandler<NavigationView, CUINavigationView>
    {
        protected override CUINavigationView CreateView(Context context)
        {
            var view = new CUINavigationView(context);

            if (VirtualView != null)
            {
                view.SetRoot(VirtualView?.Content);
                VirtualView.PerformNavigate = view.NavigateTo;
            }

            return view;
        }

        public override void Remove(View view)
        {
            if (VirtualView != null)
            {
                VirtualView.PerformNavigate = null;
            }

            base.Remove(view);
        }
    }
}

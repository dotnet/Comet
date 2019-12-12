using System;
using System.Linq;
using Android.Content;
using Comet.Android.Controls;

namespace Comet.Android.Handlers
{
	public class NavigationViewHandler : AbstractHandler<NavigationView, CUINavigationView>
	{
		CUINavigationView navigationView;
		protected override CUINavigationView CreateView(Context context)
		{
			navigationView ??= new CUINavigationView(context);

			if (VirtualView != null)
			{
				navigationView.SetRoot(VirtualView?.Content);
				VirtualView.SetPerformNavigate(navigationView.NavigateTo);
				VirtualView.SetPerformPop(navigationView.Pop);
			}

			return navigationView;
		}
		public override void SetView(View view)
		{
			var nav = view as NavigationView;
			if (navigationView != null)
			{
				navigationView.SetRoot(nav.Content);
				VirtualView.SetPerformNavigate(navigationView.NavigateTo);
				VirtualView.SetPerformPop(navigationView.Pop);
			}
			base.SetView(view);
		}
		public override void Remove(View view)
		{
			if (VirtualView != null)
			{
				VirtualView.SetPerformNavigate(action:null);
				VirtualView.SetPerformPop(action: null);
			}

			base.Remove(view);
		}
	}
}

using System;
using System.Linq;
using Android.Content;
using System.Maui.Android.Controls;

namespace System.Maui.Android.Handlers
{
	public class NavigationViewHandler : AbstractHandler<NavigationView, MauiNavigationView>
	{
		MauiNavigationView navigationView;
		protected override MauiNavigationView CreateView(Context context)
		{
			navigationView ??= new MauiNavigationView(context);

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
			if (navigationView != null && VirtualView!=null)
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

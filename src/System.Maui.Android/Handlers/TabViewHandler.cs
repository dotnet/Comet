using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using AView = Android.Views.View;
using System.Maui.Android.Controls;
using Android.Content;
using Android.Util;
using System.Linq;

namespace System.Maui.Android.Handlers
{
	public class TabViewHandler : AbstractHandler<TabView, MauiTabView>
	{
		//private AView _view;

		protected override MauiTabView CreateView(Context context)
		{
			var tabView = new MauiTabView(context);

			if (VirtualView != null)
			{
				VirtualView.ChildrenChanged += HandleChildrenChanged;
				VirtualView.ChildrenAdded += HandleChildrenChanged;
				VirtualView.ChildrenRemoved += HandleChildrenChanged;
			}

			tabView.CreateTabs(VirtualView?.ToList());

			return tabView;
		}

		public override void Remove(View view)
		{
			if (VirtualView != null)
			{
				VirtualView.ChildrenChanged -= HandleChildrenChanged;
				VirtualView.ChildrenAdded -= HandleChildrenChanged;
				VirtualView.ChildrenRemoved -= HandleChildrenChanged;
			}

			base.Remove(view);
		}

		private void HandleChildrenChanged(object sender, LayoutEventArgs e)
		{
			TypedNativeView?.CreateTabs(VirtualView?.ToList());
		}
	}
}

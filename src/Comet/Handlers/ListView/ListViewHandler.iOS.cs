using System;
using Foundation;
using UIKit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using Comet.iOS;


namespace Comet.Handlers
{
	public partial class ListViewHandler : ViewHandler<IListView, CUITableView>
	{
		

		public static void MapListViewProperty(IViewHandler viewHandler, IListView virtualView)
		{
			var nativeView = (CUITableView)viewHandler.NativeView;
			nativeView.ListView = virtualView;
			nativeView.SizeToFit();
		}

		public static void MapReloadData(IViewHandler viewHandler, IListView virtualView)
		{
			var nativeView = (CUITableView)viewHandler.NativeView;
			nativeView?.ReloadData();
		}

		protected override CUITableView CreateNativeView() =>  new CUITableView(MauiContext);
	}
}

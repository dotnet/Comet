using System;
using Foundation;
using UIKit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using Comet.iOS;


namespace Comet.Handlers
{
	public class ListViewHandler : ViewHandler<ListView, CUITableView>
	{
		public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>(ViewHandler.ViewMapper)
		{
			["ListView"] = MapListViewProperty,
			[nameof(ListView.ReloadData)] = MapReloadData
		};

		public ListViewHandler() : base(Mapper)
		{
			
		}

		public static void MapListViewProperty(IViewHandler viewHandler, ListView virtualView)
		{
			var nativeView = (CUITableView)viewHandler.NativeView;
			nativeView.ListView = virtualView;
			nativeView.SizeToFit();
		}

		public static void MapReloadData(IViewHandler viewHandler, ListView virtualView)
		{
			var nativeView = (CUITableView)viewHandler.NativeView;
			nativeView?.ReloadData();
		}

		protected override CUITableView CreateNativeView() => NativeView as CUITableView ?? new CUITableView(MauiContext);
	}
}

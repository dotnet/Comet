using System;
using Foundation;
using Comet.iOS.Controls;
using UIKit;

// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.iOS.Handlers
{
	public class ListViewHandler : AbstractHandler<ListView, CUITableView>
	{
		public override bool IgnoreSafeArea => VirtualView?.GetIgnoreSafeArea(true) ?? true;

		public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>(ViewHandler.Mapper)
		{
			["ListView"] = MapListViewProperty,
			[nameof(ListView.ReloadData)] = MapReloadData
		};

		public ListViewHandler() : base(Mapper)
		{

		}

		//Resuse the old control if we can!
		protected override CUITableView CreateView()
			=> NativeView as CUITableView ?? new CUITableView();

		public override void Remove(View view)
		{
			TypedNativeView.ListView = null;
			base.Remove(view);
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
	}
}

using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using AView = Android.Views.View;
using System.Maui.Android.Controls;
using System.Drawing;
using Android.Content;

namespace System.Maui.Android.Handlers
{
	public class ListViewHandler : AbstractControlHandler<ListView, MauiRecyclerView>
	{
		public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>(ViewHandler.Mapper)
		{
			["ListView"] = MapListViewProperty,
			[nameof(ListView.ReloadData)] = MapReloadData,
			
		};
		
		public ListViewHandler() : base(Mapper)
		{

		}
		
		protected override MauiRecyclerView CreateView(Context context) => new MauiRecyclerView(context);

		public override void Remove(View view)
		{
			TypedNativeView.ListView = null;
			base.Remove(view);
		}
		
		protected override void DisposeView(MauiRecyclerView nativeView) => nativeView.ListView = null;

		public override SizeF GetIntrinsicSize(SizeF availableSize)
		{
			//base.GetIntrinsicSize(availableSize);
			return System.Maui.View.UseAvailableWidthAndHeight;
		}
		
		public static void MapListViewProperty(IViewHandler viewHandler, ListView virtualView)
		{
			var nativeView = (MauiRecyclerView)viewHandler.NativeView;
			nativeView.ListView = virtualView;
		}

		public static void MapReloadData(IViewHandler viewHandler, ListView virtualView)
		{
			var nativeView = (MauiRecyclerView)viewHandler.NativeView;
			nativeView?.ReloadData();
		}
	}
}

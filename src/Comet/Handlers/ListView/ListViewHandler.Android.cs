using System;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AView = Android.Views.View;
using Comet.Android.Controls;
using System.Drawing;
using Android.Content;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;

namespace Comet.Handlers
{
	public partial class ListViewHandler : ViewHandler<IListView, CometRecyclerView>
	{
		
		protected override CometRecyclerView CreatePlatformView() => new CometRecyclerView(MauiContext);


		public override Microsoft.Maui.Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint) => new Microsoft.Maui.Graphics.Size(widthConstraint, heightConstraint);
		//public override SizeF GetIntrinsicSize(SizeF availableSize)
		//{
		//	//base.GetIntrinsicSize(availableSize);
		//	return Comet.View.UseAvailableWidthAndHeight;
		//}

		public static void MapListViewProperty(IElementHandler viewHandler, IListView virtualView)
		{
			var nativeView = (CometRecyclerView)viewHandler.PlatformView;
			nativeView.ListView = virtualView;
		}

		public static void MapReloadData(ListViewHandler viewHandler, IListView virtualView, object? value)
		{
			var nativeView = (CometRecyclerView)viewHandler.PlatformView;
			nativeView?.ReloadData();
		}
	}
}

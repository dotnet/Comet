//using System;
//using Android.Views;
//using Android.Widget;
//using Android.Support.V7.Widget;
//using AView = Android.Views.View;
//using Comet.Android.Controls;
//using System.Drawing;
//using Android.Content;

//namespace Comet.Android.Handlers
//{
//	public class ListViewHandler : AbstractControlHandler<ListView, CometRecyclerView>
//	{
//		public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>(ViewHandler.Mapper)
//		{
//			["ListView"] = MapListViewProperty,
//			[nameof(ListView.ReloadData)] = MapReloadData,
			
//		};
		
//		public ListViewHandler() : base(Mapper)
//		{

//		}
		
//		protected override CometRecyclerView CreateView(Context context) => new CometRecyclerView(context);

//		public override void Remove(View view)
//		{
//			TypedNativeView.ListView = null;
//			base.Remove(view);
//		}
		
//		protected override void DisposeView(CometRecyclerView nativeView) => nativeView.ListView = null;

//		public override SizeF GetIntrinsicSize(SizeF availableSize)
//		{
//			//base.GetIntrinsicSize(availableSize);
//			return Comet.View.UseAvailableWidthAndHeight;
//		}
		
//		public static void MapListViewProperty(IViewHandler viewHandler, ListView virtualView)
//		{
//			var nativeView = (CometRecyclerView)viewHandler.NativeView;
//			nativeView.ListView = virtualView;
//		}

//		public static void MapReloadData(IViewHandler viewHandler, ListView virtualView)
//		{
//			var nativeView = (CometRecyclerView)viewHandler.NativeView;
//			nativeView?.ReloadData();
//		}
//	}
//}

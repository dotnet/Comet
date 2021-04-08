using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.Android.Controls
{
	public class CometRecyclerViewAdapter : RecyclerView.Adapter
	{
		public IMauiContext MauiContext{get;set;}
		public IListView ListView { get; set; }

		//TODO: Account for Section
		public override int ItemCount => ListView?.Rows(0) ?? 0;

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			//TODO: Account for Section
			var view = ListView?.ViewFor(0, position);
			var cell = view?.ToNative(MauiContext);

			if (holder is CometRecyclerViewHolder rvh && cell != null)
			{
				Logger.Debug($"OnBindViewHolder");

				// If the cell has a cometview parent already, remove it from that parent.
				//if (cell.Parent is CometView cometParent)
				//	if (cometParent.CurrentView != view)
				//		cometParent.CurrentView = null;

				var parent = rvh.Parent;
				//TODO: Bring back DisplayScale
				var density = 1f;// AndroidContext.DisplayScale;

				var scaledSize = new Size(parent.Width / density, parent.Height / density);
				var measuredSize = view.Measure(scaledSize, true);
				view.MeasuredSize = measuredSize;
				view.MeasurementValid = true;

				rvh.CometView.LayoutParameters = new ViewGroup.LayoutParams(
					ViewGroup.LayoutParams.MatchParent,
					(int)(measuredSize.Height * density));

				// Add our view to the cell.
				rvh.CometView.CurrentView = view;
			}
			else
			{
				Logger.Warn("Should never happen.");
			}

		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			return new CometRecyclerViewHolder(parent, ListView, MauiContext);
		}
	}
}
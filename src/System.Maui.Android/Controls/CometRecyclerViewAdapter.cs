using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace System.Maui.Android.Controls
{
    public class MauiRecyclerViewAdapter : RecyclerView.Adapter
    {
        public IListView ListView { get; set; }

        //TODO: Account for Section
        public override int ItemCount => ListView?.Rows(0) ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            //TODO: Account for Section
            var view = ListView?.ViewFor(0, position);
            var cell = view?.ToView();

            if (holder is MauiRecyclerViewHolder rvh && cell != null)
            {
				Logger.Debug($"OnBindViewHolder");

				// If the cell has a System.Mauiview parent already, remove it from that parent.
				if (cell.Parent is MauiView MauiParent)
					if (MauiParent.CurrentView != view)
						MauiParent.CurrentView = null;

				var parent = rvh.Parent;
                var density = AndroidContext.DisplayScale;

                var scaledSize = new System.Drawing.SizeF(parent.Width / density, parent.Height / density);
                var measuredSize = view.Measure(scaledSize, true);
                view.MeasuredSize = measuredSize;
                view.MeasurementValid = true;

                rvh.MauiView.LayoutParameters = new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent, 
                    (int)(measuredSize.Height * density));
  
                // Add our view to the cell.
                rvh.MauiView.CurrentView = view;            
            }
            else
			{
				Logger.Warn("Should never happen.");
			}

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new MauiRecyclerViewHolder(parent, ListView);
        }
    }
}
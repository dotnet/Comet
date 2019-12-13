using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Comet.Android.Controls
{
    public class CometRecyclerViewAdapter : RecyclerView.Adapter
    {
        public IListView ListView { get; set; }

        //TODO: Account for Section
        public override int ItemCount => ListView?.Rows(0) ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            //TODO: Account for Section
            var view = ListView?.ViewFor(0, position);
            var cell = view?.ToView();

            if (holder is CometRecyclerViewHolder rvh && cell != null)
            {
                var parent = rvh.Parent;

                var density = AndroidContext.DisplayScale;

                var scaledSize = new System.Drawing.SizeF(parent.Width / density, parent.Height / density);
                var measuredSize = view.Measure(scaledSize, true);
                view.MeasuredSize = measuredSize;
                view.MeasurementValid = true;

                cell.LayoutParameters = new ViewGroup.LayoutParams(parent.Width, (int)(measuredSize.Height * density));
                cell.SetMinimumHeight((int)(measuredSize.Height * density));
                    
                rvh.Container.RemoveAllViews();
                // cell may sometimes have a parent already
                (cell.Parent as FrameLayout)?.RemoveView(cell);
                rvh.Container.AddView(cell);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new CometRecyclerViewHolder(new FrameLayout(parent.Context), parent, ListView);
        }
    }
}
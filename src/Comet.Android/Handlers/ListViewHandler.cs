using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using AView = Android.Views.View;
using Comet.Android.Controls;
using System.Drawing;

namespace Comet.Android.Handlers
{
	public class ListViewHandler : RecyclerView, AndroidViewHandler
	{
		public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		public ListViewHandler() : base(AndroidContext.CurrentContext)
		{
			var layoutManager = new LinearLayoutManager(this.Context);

			SetLayoutManager(layoutManager);
			this.SetAdapter(new RecyclerViewAdapter());
			this.AddItemDecoration(new DividerItemDecoration(Context, layoutManager.Orientation));
		}

		public AView View => this;
		public object NativeView => View;
		public bool HasContainer { get; set; } = false;

		public CUITouchGestureListener GestureListener { get; set; }

		public SizeF Measure(SizeF availableSize)
		{
			return availableSize;
		}

		public void SetFrame(RectangleF frame)
		{
			// Do nothing
		}

		public void Remove(View view)
		{
			ViewHandler.RemoveGestures(this, view);
		}

		public void SetView(View view)
		{
			((RecyclerViewAdapter)this.GetAdapter()).ListView = view as ListView;
			ViewHandler.AddGestures(this, view);
		}

		public void UpdateValue(string property, object value)
		{
			switch (property)
			{
				case nameof(ListView.ReloadData):
					((RecyclerViewAdapter)this.GetAdapter()).NotifyDataSetChanged();
					break;
				case Gesture.AddGestureProperty:
					ViewHandler.AddGesture(this, (Gesture)value);
					break;
				case Gesture.RemoveGestureProperty:
					ViewHandler.RemoveGesture(this, (Gesture)value);
					break;
			}
		}

		class RecyclerViewAdapter : Adapter
		{
			public IListView ListView { get; set; }

			//TODO: Account for Section
			public override int ItemCount => ListView?.Rows(0) ?? 0;

			public override void OnBindViewHolder(ViewHolder holder, int position)
			{
				//TODO: Account for Section
				var view = ListView?.ViewFor(0, position);
				var cell = view?.ToView();

				if (holder is RecyclerViewHolder rvh && cell != null)
				{
					var parent = rvh.Parent;

					var displayMetrics = parent.Context.Resources.DisplayMetrics;
					var density = displayMetrics.Density;

					var scaledSize = new SizeF(parent.Width / density, parent.Height / density);
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

			public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
			{
				return new RecyclerViewHolder(new FrameLayout(parent.Context), parent, ListView);
			}
		}

		public class RecyclerViewHolder : ViewHolder
		{
			private readonly IListView listView;

			public FrameLayout Container { get; }
			public ViewGroup Parent { get; }

			public RecyclerViewHolder(FrameLayout itemView, ViewGroup parent, IListView listView)
				: base(itemView)
			{
				Container = itemView;
				Parent = parent;
				this.listView = listView;

				Container.Click += HandleClick;
			}

			private void HandleClick(object sender, EventArgs e)
			{
				listView?.OnSelected(0, this.AdapterPosition);
			}
		}
	}
}

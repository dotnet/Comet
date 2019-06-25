using System;
using Foundation;
using UIKit;
namespace HotUI.iOS {
	public class ListViewHandler : UITableView, IUIView, IUITableViewDataSource {
		public UIView View => this;

		public ListViewHandler ()
		{
			this.WeakDataSource = this;
		}
		ListView listView;

		public void SetView (View view)
		{
			listView = view as ListView;
			//TODO: Some crude size estimation
			var v = listView.CellCreator (listView.List [0]);
			this.EstimatedRowHeight = 200;
			this.ReloadData ();
		}

		public void UpdateValue (string property, object value)
		{
			this.ReloadData ();
			
		}

		public void Remove (View view)
		{
			this.ReloadData ();
		}

		public override nint NumberOfSections () => listView?.List?.Count  > 0 ? 1 :0;

		public override nint NumberOfRowsInSection (nint section) => listView?.List?.Count ?? 0;


		static readonly string cellType = "ViewCell";
		

		public nint RowsInSection (UITableView tableView, nint section) => listView?.List?.Count ?? 0;

		public UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = this.DequeueReusableCell (cellType) as ViewCell ?? new ViewCell ();
			var item = listView?.List [indexPath.Row];
			var v = listView?.CellCreator (item);
			cell.SetView (v);
			return cell;
		}

		class ViewCell : UITableViewCell {
			UIView currentContent;

			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();
				if (currentContent == null)
					return;
				currentContent.Frame = ContentView.Bounds;
			}
			View currentView;
			public void SetView(View view)
			{

				//TODO:We should do View Compare
				//view.Diff (view);
				currentContent?.RemoveFromSuperview ();
				currentContent = view.ToView ();
				ContentView.Add (currentContent);
				//This should let it autosize
				NSLayoutConstraint.ActivateConstraints (new []{
				currentContent.LeadingAnchor.ConstraintEqualTo (this.LeadingAnchor),
				currentContent.TrailingAnchor.ConstraintEqualTo (this.TrailingAnchor),
				currentContent.TopAnchor.ConstraintEqualTo (this.TopAnchor),
				currentContent.BottomAnchor.ConstraintEqualTo (this.BottomAnchor),
			});
			}
		}
	}
}

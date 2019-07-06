using System;
using AppKit;
using CoreGraphics;
using Foundation;
using HotUI.Mac.Extensions;

namespace HotUI.Mac {
	public class ListViewHandler : NSColorView, INSView, INSTableViewDataSource, INSTableViewDelegate {


		NSTableView TableView;
		NSScrollView tableViewContainer;
		public ListViewHandler()
		{
			TableView = new NSTableView (new CGRect (0, 0, 500, 500));

			TableView.AddColumn (new NSTableColumn ("ListView"));
			TableView.HeaderView = null;
			//TableView.UsesAutomaticRowHeights = true;
			TableView.RowHeight = 44;
			TableView.SizeLastColumnToFit ();
			TableView.WeakDataSource = this;
			TableView.WeakDelegate = this;

			AddSubview (tableViewContainer = new NSScrollView (new CGRect (0, 0, 500, 500)));
			tableViewContainer.DocumentView = TableView;
			tableViewContainer.HasVerticalScroller = true;
		}


		public NSView View => this;

		public void Remove (View view)
		{
			listView = null;
			TableView.ReloadData ();
		}
		ListView listView;
		public void SetView (View view)
		{
			listView = view as ListView;
			if (listView == null)
				return;
			cellIdentifier = listView.GetType ().Name;
			TableView.ReloadData ();
            TableView.SizeLastColumnToFit();
        }

        public void UpdateValue (string property, object value)
		{
			TableView.ReloadData ();
            TableView.SizeLastColumnToFit();
        }

        string cellIdentifier = "viewCell";
		[Export ("tableView:viewForTableColumn:row:")]
		public NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
			var cell = tableView.MakeView (cellIdentifier, this) as ViewCell ?? new ViewCell ();
			var item = listView?.List [(int)row];
			var v = listView?.CellCreator (item);
			v.Parent = listView;
			cell.SetView (v);
			return cell;
		}

		[Export ("tableView:objectValueForTableColumn:row:")]
		public Foundation.NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
			var item = listView?.List [(int)row];
			return new NSString (item?.ToString ());
		}

		[Export ("numberOfRowsInTableView:")]
		public nint GetRowCount (NSTableView tableView) => listView?.List?.Count ?? 0;

		[Export ("tableViewSelectionDidChange:")]
		public void SelectionDidChange (Foundation.NSNotification notification)
		{
			var row = TableView.SelectedRow;
			if (row < 0)
				return;
			TableView.DeselectAll (this);
			listView?.OnSelected ((int)row);
		}

		[Export ("tableView:didAddRowView:forRow:")]
		public void DidAddRowView (NSTableView tableView, NSTableRowView rowView, nint row)
		{
			Console.WriteLine (rowView.Frame);
		}


		public override void Layout ()
		{
			base.Layout ();
			tableViewContainer.Frame = TableView.Frame = Bounds;
		}

		class ViewCell : NSTableCellView {

			NSView currentContent;
			View currentView;
			public void SetView (View view)
			{
				//TODO:We should do View Compare
				//view.Diff (view);
				currentContent?.RemoveFromSuperview ();
				currentContent = view.ToView ();
				currentView = view;
				Console.WriteLine (currentContent.FittingSize);
				this.AddSubview (currentContent);
			}

			public override void Layout ()
			{
				base.Layout ();
				if (currentContent == null)
					return;
				currentContent.Frame = Bounds;
			}

		}
	}
}

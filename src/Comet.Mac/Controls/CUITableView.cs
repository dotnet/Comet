using System;
using AppKit;
using CoreGraphics;

namespace Comet.Mac.Controls
{
	public class CUITableView : NSScrollView
	{
		private NSTableView _tableView;
		private CUITableViewSource _delegate;

		public CUITableView() : this(new CGRect(0, 0, 320, 800))
		{

		}

		public CUITableView(CGRect rect)
		{
			_tableView = new NSTableView(rect);

			_tableView.AddColumn(new NSTableColumn("ListView"));
			_tableView.HeaderView = null;
			_tableView.SizeLastColumnToFit();

			_delegate = new CUITableViewSource(_tableView);
			_tableView.WeakDataSource = _delegate;
			_tableView.WeakDelegate = _delegate;

			DocumentView = _tableView;
			HasVerticalScroller = true;
		}

		public IListView ListView
		{
			get => _delegate.ListView;
			set
			{
				_delegate.ListView = value;

				_tableView.ReloadData();
				_tableView.SizeLastColumnToFit();
			}
		}

		public bool UnevenRows
		{
			get => _delegate.UnevenRows;
			set => _delegate.UnevenRows = value;
		}

		public void ReloadData() => _tableView?.ReloadData();
	}
}

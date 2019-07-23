using System;
using AppKit;
using CoreGraphics;

namespace HotUI.Mac.Controls
{
    public class HUITableView : NSScrollView
    {
        private NSTableView _tableView;
        private HUITableViewSource _delegate;
        
        public HUITableView() : this(new CGRect(0,0,320,800))
        {

        }

        public HUITableView(CGRect rect)
        {
            _tableView = new NSTableView(rect);

            _tableView.AddColumn(new NSTableColumn("ListView"));
            _tableView.HeaderView = null;
            _tableView.SizeLastColumnToFit();

            _delegate = new HUITableViewSource(_tableView);
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

using System;
using AppKit;
using Foundation;
using Comet.Mac.Extensions;

namespace Comet.Mac.Controls
{
    public class CUITableViewSource : NSTableViewSource
    {
        private static readonly string CellType = "CUIViewCell";

        private IListView _listView;
        private bool _unevenRows;
        private float? _rowHeight;
        private WeakReference<NSTableView> _tableView;

        public CUITableViewSource(NSTableView tableView)
        {
            _tableView = new WeakReference<NSTableView>(tableView);
        }

        public IListView ListView
        {
            get => _listView;
            set
            {
                _listView = value;
                _rowHeight = null;
            }
        }

        public bool UnevenRows
        {
            get => _unevenRows;
            set
            {
                _unevenRows = value;
                _rowHeight = null;
            }
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return _listView?.Rows(0) ?? 0;
        }

        public override nfloat GetRowHeight(NSTableView tableView, nint row)
        {
            if (_unevenRows)
                return CalculateRowHeight(0, (int)row);

            if (_rowHeight == null)
                _rowHeight = CalculateRowHeight(0, (int)row);

            return (float)_rowHeight;
        }

        public override NSObject GetObjectValue(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            //TODO: Account for Sections
            var v = _listView.ViewFor(0, (int)row);
            return new NSString(v.ToString());
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            var cell = tableView.MakeView(CellType, this) as CUITableViewCell;
            if (cell == null)
			{
				cell = new CUITableViewCell();
				cell.Identifier = CellType;
			}

            //TODO: Account for Sections
            var v = _listView?.ViewFor(0, (int)row);
            cell.SetView(v);
            return cell;
        }

        public override void SelectionDidChange(NSNotification notification)
        {
            if (!_tableView.TryGetTarget(out var tableView)) return;
            
            var row = tableView.SelectedRow;
            if (row < 0) return;

            tableView.DeselectAll(this);
            //TODO: Account for Sections
            _listView?.OnSelected(0, (int)row);            
        }

        private float CalculateRowHeight(int section, int row)
        {
            _tableView.TryGetTarget(out var tableView);

            // todo: we really need a "GetOrCreate" method.
            var view = _listView?.ViewFor(section, row);
            if (view != null)
            {
                var constraints = view.GetFrameConstraints();
                if (constraints?.Height != null)
                    return (float)constraints?.Height;

                // todo: this is really inefficient.
                if (view.ToView() != null)
                {
                    var measure = view.Measure(tableView.Bounds.Size.ToSizeF());
                    return measure.Height;
                }
            }

            return 44f;
        }
    }
}

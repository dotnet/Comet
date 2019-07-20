using System;
using Foundation;
using UIKit;

namespace HotUI.iOS.Controls
{
    public class HUITableViewSource : UITableViewSource
    {
        private static readonly string CellType = "HUIViewCell";

        private IListView _listView;
        private bool _unevenRows;
        private float? _rowHeight;
        
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

        public override nint NumberOfSections(UITableView tableView) => 1;

        public override nint RowsInSection(UITableView tableview, nint section) => _listView?.Rows((int)section) ?? 0;

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            _listView?.OnSelected(indexPath.Section,indexPath.Row);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellType) as HUITableViewCell ?? new HUITableViewCell(UITableViewCellStyle.Default, CellType);
            var v = _listView?.ViewFor(indexPath.Section,indexPath.Row);
            cell.SetView(v, _listView.ShouldDisposeViews);
            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.RowHeight > 0)
                return tableView.RowHeight;

            if (_unevenRows)
                return CalculateRowHeight(tableView,indexPath.Section, indexPath.Row);
            
            if (_rowHeight == null)
                _rowHeight = CalculateRowHeight(tableView, indexPath.Section, indexPath.Row);
            
            return (float) _rowHeight;
        }

        private float CalculateRowHeight(UITableView tableView,int section, int row)
        {
            // todo: we really need a "GetOrCreate" method.
            var view = _listView?.ViewFor(section, row);
            if (view != null)
            {
                if (view is NavigationButton navigationButton)
                    view = navigationButton.Content;

                if (view.FrameConstraints?.Height != null)
                    return (float)view.FrameConstraints?.Height;

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

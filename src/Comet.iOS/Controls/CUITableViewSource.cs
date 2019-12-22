using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Comet.iOS.Controls
{
	public class CUITableViewSource : UITableViewSource
	{
		private static readonly string CellType = "CUIViewCell";

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
		public bool HasHeaders { get; set; }

		public override bool RespondsToSelector(Selector sel)
		{
			if (sel.Name == "tableView:viewForHeaderInSection:")
			{
				return HasHeaders;

			}
			return base.RespondsToSelector(sel);
		}
		public override UIView GetViewForHeader(UITableView tableView, nint section) => _listView?.HeaderFor((int)section).ToView();

		public override nint NumberOfSections(UITableView tableView) => _listView?.Sections() ?? 0;

		public override nint RowsInSection(UITableView tableview, nint section) => _listView?.Rows((int)section) ?? 0;

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			_listView?.OnSelected(indexPath.Section, indexPath.Row);
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cellIdentifier = CellType;
			var cell = tableView.DequeueReusableCell(cellIdentifier) as CUITableViewCell ?? new CUITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
			var v = _listView?.ViewFor(indexPath.Section, indexPath.Row);
			cell.SetView(v);
			return cell;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			if (tableView.RowHeight > 0)
				return tableView.RowHeight;

			if (_unevenRows)
				return CalculateRowHeight(tableView, indexPath.Section, indexPath.Row);

			if (_rowHeight == null)
				_rowHeight = CalculateRowHeight(tableView, indexPath.Section, indexPath.Row);

			return (float)_rowHeight;
		}

		private float CalculateRowHeight(UITableView tableView, int section, int row)
		{
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
					var measure = view.Measure(tableView.Bounds.Size.ToSizeF(), true);
					return measure.Height;
				}
			}


			return 44f;
		}
	}
}

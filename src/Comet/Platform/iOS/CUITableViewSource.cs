using System;
using Foundation;
using Microsoft.Maui;
using ObjCRuntime;
using UIKit;

namespace Comet.iOS
{
	public class CUITableViewSource : UITableViewSource
	{
		public CUITableViewSource(IMauiContext context)
		{
			Context = context;
		}
		private static readonly string CellType = "CUIViewCell";
		public IMauiContext Context { get; protected set; }
		private IListView _listView;
		private bool _unevenRows;
		private double? _rowHeight;

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
		public override UIView GetViewForHeader(UITableView tableView, nint section) => _listView?.HeaderFor((int)section).ToNative(Context);

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
			var cell = tableView.DequeueReusableCell(cellIdentifier, indexPath) as CUITableViewCell;
			cell.SetFromContext(Context);
			var v = _listView?.ViewFor(indexPath.Section, indexPath.Row);
			cell.SetView(v);
			Console.WriteLine(v.ToString());
			return cell;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			if (tableView.RowHeight > 0)
				return tableView.RowHeight;

			if (_unevenRows)
				return (nfloat)CalculateRowHeight(tableView, indexPath.Section, indexPath.Row);

			if (_rowHeight == null)
				_rowHeight = CalculateRowHeight(tableView, indexPath.Section, indexPath.Row);

			return (nfloat)_rowHeight;
		}

		private double CalculateRowHeight(UITableView tableView, int section, int row)
		{
			// todo: we really need a "GetOrCreate" method.
			var view = _listView?.ViewFor(section, row);
			if (view != null)
			{
				var constraints = view.GetFrameConstraints();

				if (constraints?.Height != null)
					return (float)constraints?.Height;

				// todo: this is really inefficient.
				if (view.ToNative(Context) != null)
				{
					var size = tableView.Bounds.Size;
					var measure = view.Measure(size.Width,size.Height);
					return measure.Height;
				}
			}


			return 44f;
		}
	}
}

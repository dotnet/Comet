using System;
using Foundation;
using UIKit;

namespace Comet.iOS.Controls
{
	public class CUITableView : UITableView
	{
		private CUITableViewSource _delegate;

		public CUITableView()
		{
			_delegate = new CUITableViewSource();
			WeakDataSource = _delegate;
			WeakDelegate = _delegate;
		}

		public IListView ListView
		{
			get => _delegate.ListView;
			set
			{
				ResetDelegate(value);

				// If we have items in the list, we can check to see if there are frame constraints on the root view.  If so,
				// we can use those as our cell height.
				if (value?.Sections() > 0 && value?.Rows(0) > 0)
				{

					var v = value.ViewFor(0, 0);
					var constraints = v?.GetFrameConstraints();

					if (constraints?.Height != null)
						RowHeight = (float)constraints.Height;
					else
						RowHeight = -1;
				}
				else
				{
					RowHeight = -1;
				}


				ReloadData();
			}
		}
		void ResetDelegate(IListView listView)
		{
			bool hasHeader = false;
			bool valueChanged = false;
			var uneven = _delegate.UnevenRows;
			if (listView?.Sections() > 0)
			{
				hasHeader = listView.HeaderFor(0) != null;
			}
			if (_delegate.HasHeaders != hasHeader)
			{
				valueChanged = true;
			}
			if (valueChanged)
			{
				_delegate = new CUITableViewSource
				{
					HasHeaders = hasHeader,
					UnevenRows = uneven,
					ListView = listView,
				};
				_delegate.HasHeaders = hasHeader;
				WeakDataSource = _delegate;
				WeakDelegate = _delegate;
			}
			else
				_delegate.ListView = listView;
		}
		public bool UnevenRows
		{
			get => _delegate.UnevenRows;
			set => _delegate.UnevenRows = value;
		}
	}
}

using System;
using Foundation;
using UIKit;

namespace HotUI.iOS.Controls
{
    public class HUITableView : UITableView, IUITableViewDataSource, IUITableViewDelegate
    {
        private static readonly string CellType = "ViewCell";
        private ListView _listView;

        public HUITableView()
        {
            WeakDataSource = this;
            WeakDelegate = this;
        }

        public ListView ListView
        {
            get => _listView;
            set
            {
                _listView = value;
                ReloadData();
            }
        }
        
        public nint RowsInSection(UITableView tableView, nint section)
        {
            return _listView?.List?.Count ?? 0;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = DequeueReusableCell(CellType) as ViewCell ?? new ViewCell();
            var item = _listView?.List[indexPath.Row];
            var v = _listView?.CellCreator?.Invoke(item);
            v.Parent = _listView;
            cell.SetView(v);
            return cell;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row < 0)
                return;
            tableView.DeselectRow(indexPath, true);
            _listView?.OnSelected(indexPath.Row);
        }

        private class ViewCell : UITableViewCell
        {
            private UIView _currentContent;
            private View _currentView;

            public override void LayoutSubviews()
            {
                base.LayoutSubviews();
                if (_currentContent == null)
                    return;
                _currentContent.Frame = ContentView.Bounds;
            }

            public void SetView(View view)
            {
                //TODO:We should do View Compare
                //view.Diff (view);
                _currentContent?.RemoveFromSuperview();
                _currentContent = view.ToView();
                ContentView.Add(_currentContent);
                //This should let it autosize
                NSLayoutConstraint.ActivateConstraints(new[]
                {
                    _currentContent.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                    _currentContent.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                    _currentContent.TopAnchor.ConstraintEqualTo(TopAnchor),
                    _currentContent.BottomAnchor.ConstraintEqualTo(BottomAnchor)
                });
            }
        }
    }
}
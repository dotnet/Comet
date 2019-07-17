using UIKit;

namespace HotUI.iOS.Controls
{
    public class HUITableViewCell : UITableViewCell
    {
        private UIView _currentContent;
        private View _currentView;

        public HUITableViewCell(UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
        {
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (_currentContent == null)
                return;
            
            _currentContent.Frame = ContentView.Bounds;
        }

        public void SetView(View view)
        {
            if (_currentView != null)
            {
                view = view.Diff(_currentView);
                _currentContent?.RemoveFromSuperview();
            }
            
            if (_currentView != null)
            {
                _currentView.ViewHandler = null;
                _currentView?.Dispose();
            }
            
            _currentView = view;
            _currentContent = view.ToView();
            ContentView.Add(_currentContent);
        }
    }
}
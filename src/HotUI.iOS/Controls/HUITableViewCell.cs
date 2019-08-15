using System;
using UIKit;

namespace Comet.iOS.Controls
{
    public class HUITableViewCell : UITableViewCell
    {
        public static int _instanceCount;

        private UIView _currentContent;
        WeakReference __virtualView;
        private View _virtualView
        {
            get => __virtualView?.Target as View;
            set => __virtualView = new WeakReference(value);
        }

        public HUITableViewCell(UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
        {
            ContentView.Tag = _instanceCount++;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (_currentContent == null)
                return;

            _virtualView.SetFrameFromNativeView(ContentView.Bounds.ToRectangleF());
        }

        public void SetView(View view, bool shouldDispose)
        { 
            var oldView = _virtualView;
            var isFromThisCell = oldView?.ToView() == _currentContent;
            //Apple bug, somehow the view be a weird recycle... So only re-use if the view still matches
            if (isFromThisCell && _virtualView != null && !_virtualView.IsDisposed)
            {
                view = view.Diff(_virtualView);
            }

            if (shouldDispose)
                _virtualView?.Dispose();

            _virtualView = view;
            var newView = view.ToView();

            if (_currentContent != null && _currentContent != newView)
            {
                if (_currentContent is UILabel button)
                    Logger.Debug($"[{ContentView.Tag}] Removing label: {button.Text}");
                _currentContent?.RemoveFromSuperview();
                if (newView is UILabel newButton)
                    Logger.Debug($"[{ContentView.Tag}] Replaced with label: {newButton.Text}");
            }
            else
            {
                if (newView is UILabel newButton)
                    Logger.Debug($"[{ContentView.Tag}] Rendering label: {newButton.Text}");
            }

            _currentContent = newView;
            if (_currentContent != null && _currentContent.Superview != ContentView)
                ContentView.Add(_currentContent);
        }
    }
}
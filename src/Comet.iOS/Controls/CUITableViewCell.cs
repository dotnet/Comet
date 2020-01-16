using System;
using UIKit;

namespace Comet.iOS.Controls
{
	public class CUITableViewCell : UITableViewCell
	{
		public static int _instanceCount;

		CometView cometView;

		public CUITableViewCell(UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
		{
			ContentView.Tag = _instanceCount++;

			cometView = new CometView(ContentView.Bounds);
			cometView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			ContentView.AutosizesSubviews = true;
			ContentView.AddSubview(cometView);
		}

		public void SetView(View view)
		{
			var previousView = cometView.CurrentView;
			var isFromThisCell = previousView == view;

			//Apple bug, somehow the view be a weird recycle... So only re-use if the view still matches
			if (isFromThisCell && previousView != null && !previousView.IsDisposed)
				view = view.Diff(previousView, false);

			cometView.CurrentView = view;

			previousView?.ViewDidDisappear();
			view?.ViewDidAppear();
		}
	}
}

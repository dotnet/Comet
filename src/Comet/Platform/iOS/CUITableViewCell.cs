using System;
using Foundation;
using Microsoft.Maui;
using UIKit;

namespace Comet.iOS
{
	[Register("CUITableViewCell")]
	public class CUITableViewCell : UITableViewCell
	{
		public static int _instanceCount;

		CometView cometView;
		public CUITableViewCell(IntPtr ptr) : base(ptr)
		{
			ContentView.Tag = _instanceCount++;
		}

		public void SetFromContext(IMauiContext context)
		{
			if (cometView != null)
				return;

			ContentView.Tag = _instanceCount++;
			cometView = new CometView(ContentView.Bounds, context);
			cometView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			ContentView.AutosizesSubviews = true;
			ContentView.AddSubview(cometView);
		}

		public CUITableViewCell(IMauiContext context, UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
		{
			SetFromContext(context);
		}

		public void SetView(View view)
		{
			var previousView = cometView.CurrentView;
			var isFromThisCell = previousView == view;

			//Apple bug, somehow the view be a weird recycle... So only re-use if the view still matches
			//if (isFromThisCell && previousView != null && !previousView.IsDisposed)
			//	view = view.Diff(previousView, false);

			cometView.CurrentView = view;

			//previousView?.ViewDidDisappear();
			view?.ViewDidAppear();
		}
	}
}

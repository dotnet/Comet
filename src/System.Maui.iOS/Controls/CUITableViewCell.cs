using System;
using UIKit;

namespace System.Maui.iOS.Controls
{
	public class CUITableViewCell : UITableViewCell
	{
		public static int _instanceCount;

		MauiView MauiView;

		public CUITableViewCell(UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
		{
			ContentView.Tag = _instanceCount++;

			MauiView = new MauiView(ContentView.Bounds);
			MauiView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			ContentView.AutosizesSubviews = true;
			ContentView.AddSubview(MauiView);
		}

		public void SetView(View view)
		{
			var previousView = MauiView.CurrentView;
			var isFromThisCell = previousView == view;

			//Apple bug, somehow the view be a weird recycle... So only re-use if the view still matches
			if (isFromThisCell && previousView != null && !previousView.IsDisposed)
				view = view.Diff(previousView, false);

			MauiView.CurrentView = view;

			previousView?.ViewDidDisappear();
			view.ViewDidAppear();
		}
	}
}

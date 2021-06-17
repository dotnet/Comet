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
			BackgroundColor = UIColor.Clear;
		}

		public CUITableViewCell(NSString resuseIdentifier) : base(UITableViewCellStyle.Default,resuseIdentifier)
		{
			ContentView.Tag = _instanceCount++;
			BackgroundColor = UIColor.Clear;
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
			cometView.CurrentView = view;
			(previousView as View)?.ViewDidDisappear();
			view?.ViewDidAppear();
		}
	}
}

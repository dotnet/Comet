using System;
using UIKit;
namespace Comet.iOS
{
	public static class iOSExtensions
	{
		public static UIViewController GetViewController(this UIView view)
		{
			if (view.NextResponder is UIViewController vc)
				return vc;
			if (view.NextResponder is UIView uIView)
				return uIView.GetViewController();
			return null;
		}

		public static UIGestureRecognizer ToGestureRecognizer(this Gesture gesture)
		{
			if (gesture is TapGesture tapGesture)
			{
				return new CUITapGesture(tapGesture);
			}
			throw new NotImplementedException();
		}
	}
}

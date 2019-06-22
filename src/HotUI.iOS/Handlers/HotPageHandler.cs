using System;
using UIKit;

namespace HotUI.iOS {
	public class HotPageHandler : UIViewController, IUIViewController {

		public UIViewController ViewController => this;

		UIView currentView;
		public void Remove (View view)
		{
			currentView?.RemoveFromSuperview ();
		}

		public void SetView (View view)
		{
			currentView?.RemoveFromSuperview ();
			currentView = view.ToView ();
			View.Add (currentView);
		}
		HotPage hotpage;
		public void SetViewBuilder (ViewBuilder builder)
		{
			hotpage = builder as HotPage;
			if (hotpage.View == null)
				hotpage.ReBuildView ();
			this.UpdateProperties (hotpage);
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();
			if (currentView == null)
				return;
			if(currentView is UIScrollView)
				currentView.Frame = View.Bounds;
			else {
				//TODO: opt out of safe are
				var bounds = View.Bounds;
				var safe = View.SafeAreaInsets;
				bounds.X += safe.Left;
				bounds.Y += safe.Top;
				bounds.Height -= safe.Top + safe.Bottom;
				bounds.Width -= safe.Left + safe.Right;
				currentView.Frame = bounds;
			}
		}
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			hotpage?.OnAppearing ();
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			hotpage?.OnDisppearing ();
		}
		public override void LoadView ()
		{
			base.LoadView ();
			View.BackgroundColor = UIColor.White;
		}
	}

	public static partial class ControlExtensions {

		public static void UpdateProperties (this UIViewController view, HotPage hView)
		{
			view.Title = hView.Title;
		}

		public static bool UpdateProperty (this UIViewController view, string property, object value)
		{
			switch (property) {
			case nameof (HotPage.Title):
				view.Title = (string)value;
				return true;
			}
			return false;
		}
	}
}

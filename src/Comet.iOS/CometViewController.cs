using System;
using System.Drawing;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;

namespace Comet.iOS
{
	public class CometViewController : UIViewController, IViewHandler
	{
		private CometView _containerView;
		private View _startingCurrentView;

		public View CurrentView
		{
			get => _containerView?.CurrentView ?? _startingCurrentView;
			set
			{
				if (_containerView != null)
					_containerView.CurrentView = value;
				else
					_startingCurrentView = value;

				Title = value?.GetTitle() ?? "";

			}
		}

		public object NativeView => null;

		public bool HasContainer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		bool wasPopped;
		public void WasPopped() => wasPopped = true;

		public override void LoadView()
		{
			base.View = _containerView = new CometView(UIScreen.MainScreen.Bounds);
			_containerView.CurrentView = _startingCurrentView;
			Title = _startingCurrentView?.GetTitle() ?? "";
			_startingCurrentView = null;
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			_containerView?.CurrentView?.ViewDidAppear();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ApplyStyle();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			_containerView?.CurrentView?.ViewDidDisappear();
			if (wasPopped)
			{
				CurrentView?.Dispose();
				CurrentView = null;
			}
		}

		public void ApplyStyle()
		{
			var barColor = _containerView?.CurrentView?.GetNavigationBackgroundColor()?.ToUIColor() ?? CUINavigationController.DefaultBarTintColor;

			if (NavigationController != null)
			{
				this.NavigationController.NavigationBar.BarTintColor = barColor;
			}

			var textColor = _containerView?.CurrentView?.GetNavigationTextColor()?.ToUIColor() ?? CUINavigationController.DefaultTintColor;
			if (NavigationController != null)
			{
				this.NavigationController.NavigationBar.TintColor = textColor;
				this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes { ForegroundColor = textColor };
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				CurrentView?.Dispose();
				CurrentView = null;
			}
			base.Dispose(disposing);
		}

		public void SetView(View view) => CurrentView = view;

		public void UpdateValue(string property, object value)
		{

		}

		public void Remove(View view)
		{
		}

		public SizeF GetIntrinsicSize(SizeF availableSize) => availableSize;

		public void SetFrame(RectangleF frame)
		{

		}
	}
}

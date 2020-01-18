using System;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;

namespace Comet.Mac
{
	public class NSNavigationController : NSViewController
	{
		TitleBar Toolbar;
		NSView MainContentView;
		NSViewController currentView;
		Stack<NSViewController> BackStack = new Stack<NSViewController>();
		public NSNavigationController(NSViewController currentView) : this()
		{
			PushViewController(currentView, false);
		}
		public NSNavigationController()
		{
			View = new NSColorView();
			View.AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;
			View.AddSubview(MainContentView = new NSColorView { BackgroundColor = NSColor.White });
			MainContentView.AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;
			View.AddSubview(Toolbar = new TitleBar());
		}

		public void PushViewController(NSViewController viewController, bool animated)
		{
			BackStack.Push(viewController);
			SwitchContent(viewController);
		}

		public void Pop()
		{
			var vc = BackStack.Pop();

			var cometVC = vc as CometViewController;
			if (cometVC?.CurrentView != null)
			{
				cometVC?.CurrentView?.Dispose();
				cometVC.CurrentView = null;
			}

			var next = BackStack.Peek();
			SwitchContent(next);
		}

		protected void SwitchContent(NSViewController viewController)
		{
			if (currentView != null)
			{
				currentView.View.RemoveFromSuperview();
				var childIndex = Array.IndexOf(ChildViewControllers, currentView);
				this.RemoveChildViewController(childIndex);
			}

			//TODO: Get Title
			//var navItem = view as INavigationItem;
			//Toolbar.Title = navItem?.Title ?? "";
			//if (navItem != null) {
			//	navItem.NavigationController = this;
			//}
			Toolbar.Title = "Test";
			Toolbar.BackButtonHidden = BackStack.Count <= 1;



			viewController.View.Frame = MainContentView.Bounds;
			viewController.View.AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;
			this.AddChildViewController(viewController);
			MainContentView.AddSubview(viewController.View);
			currentView = viewController;

		}
		public override void ViewDidLayout()
		{
			base.ViewDidLayout();
			var bounds = View.Bounds;
			var frame = Toolbar.Frame;
			frame.X = 0;
			frame.Y = 0;
			frame.Width = bounds.Width;
			Toolbar.Frame = frame;

			frame.Y = frame.Bottom;
			frame.Height = bounds.Height - frame.Y;
			MainContentView.Frame = frame;
			if (currentView != null)
			{
				currentView.View.Frame = bounds;
			}
		}

		public override void ViewDidAppear()
		{
			base.ViewDidAppear();
			Toolbar.BackButtonPressed = Pop;
		}

		public override void ViewDidDisappear()
		{
			base.ViewDidDisappear();
			Toolbar.BackButtonPressed = null;
		}




		class TitleBar : NSColorView
		{
			public string Title
			{
				get
				{
					return titleField.StringValue;
				}
				set
				{
					titleField.StringValue = value;
					ResizeSubviewsWithOldSize(Bounds.Size);
				}
			}

			public bool BackButtonHidden
			{
				get
				{
					return backButton.Hidden;
				}
				set
				{
					backButton.Hidden = value;
				}
			}

			public Action BackButtonPressed { get; set; }

			NSTextField titleField;
			NSButton backButton;
			public TitleBar() : base(new CGRect(9, 0, 100, 44))
			{
				AddSubview(titleField = new NSTextField());
				AddSubview(backButton = new NSButton() { Title = "Back" });
				backButton.Activated += (object sender, EventArgs e) => BackButtonPressed?.Invoke();
				BackgroundColor = NSColor.Gray;

			}
			public override bool IsFlipped
			{
				get
				{
					return true;
				}
			}

			public override void ResizeSubviewsWithOldSize(CGSize oldSize)
			{
				base.ResizeSubviewsWithOldSize(oldSize);
				var bounds = Bounds;

				titleField.SizeToFit();
				var frame = titleField.Frame;
				frame.X = (bounds.Width - frame.Width) / 2;
				frame.Y = (bounds.Height - frame.Height) / 2;

				titleField.Frame = frame;

				backButton.SizeToFit();
				frame = backButton.Frame;
				frame.X = 10f;
				frame.Y = (bounds.Height - frame.Height) / 2;
				backButton.Frame = frame;

			}
		}
	}
}


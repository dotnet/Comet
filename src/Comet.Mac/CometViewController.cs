using System;
using System.Drawing;
using AppKit;
using Comet.Mac.Extensions;

namespace Comet.Mac
{
	public class CometViewController : NSViewController, IViewHandler
	{
		private CometView _containerView;

		public CometViewController()
		{
		}

		public View CurrentView
		{
			get => ContainerView.CurrentView;
			set => ContainerView.CurrentView = value;
		}

		public override void LoadView()
		{
			base.View = ContainerView;
		}

		void IViewHandler.SetView(View view) => CurrentView = view;
		void IViewHandler.UpdateValue(string property, object value)
		{

		}

		void IViewHandler.Remove(View view)
		{

		}

		SizeF IViewHandler.GetIntrinsicSize(SizeF availableSize)
			=> availableSize;
		void IViewHandler.SetFrame(RectangleF frame)
		{

		}

		private CometView ContainerView
		{
			get
			{
				if (_containerView == null)
					_containerView = new CometView(NSScreen.MainScreen.Frame);

				return _containerView;
			}
		}

		object IViewHandler.NativeView => null;

		bool IViewHandler.HasContainer { get ; set; }
	}
}

using System;
using CoreGraphics;
using Comet.iOS.Controls;
using UIKit;
using System.Drawing;

namespace Comet.iOS.Handlers
{
	public class AbstractLayoutHandler : UIView, iOSViewHandler
	{
		public static readonly PropertyMapper<AbstractLayout> Mapper = new PropertyMapper<AbstractLayout>(ViewHandler.Mapper);

		private AbstractLayout _view;
		private SizeF _measured;

		public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		protected AbstractLayoutHandler(CGRect rect) : base(rect)
		{
		}

		protected AbstractLayoutHandler()
		{

		}

		public UIView View => this;

		public CUIContainerView ContainerView => null;

		public object NativeView => this;

		public bool HasContainer
		{
			get => false;
			set { }
		}
		
		public SizeF GetIntrinsicSize(SizeF availableSize) => Comet.View.UseAvailableWidthAndHeight;

		public void SetFrame(RectangleF frame)
		{
			Frame = frame.ToCGRect();
		}

		public void SetView(View view)
		{
			_view = view as AbstractLayout;
			if (_view != null)
			{
				_view.NeedsLayout += HandleNeedsLayout;
				_view.ChildrenChanged += HandleChildrenChanged;
				_view.ChildrenAdded += HandleChildrenAdded;
				_view.ChildrenRemoved += ViewOnChildrenRemoved;

				foreach (var subview in _view)
				{
					subview.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
					if (subview.ViewHandler is iOSViewHandler handler)
						handler.NativeViewChanged += HandleSubviewNativeViewChanged;

					var nativeView = subview.ToView() ?? new UIView();
					AddSubview(nativeView);
				}

				SetNeedsLayout();
			}

			Mapper?.UpdateProperties(this, _view);
			ViewHandler.AddGestures(this, view);
		}

		private void HandleNeedsLayout(object sender, EventArgs e)
		{
			SetNeedsLayout();
		}

		public void Remove(View view)
		{
			ViewHandler.RemoveGestures(this, view);
			foreach (var subview in _view)
			{
				subview.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
				if (subview.ViewHandler is iOSViewHandler handler)
					handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
			}

			_view.NeedsLayout -= HandleNeedsLayout;
			_view.ChildrenChanged -= HandleChildrenChanged;
			_view.ChildrenAdded -= HandleChildrenAdded;
			_view.ChildrenRemoved -= ViewOnChildrenRemoved;
			_view = null;
		}

		private void HandleSubviewViewHandlerChanged(object sender, ViewHandlerChangedEventArgs e)
		{
			if (e.OldViewHandler is iOSViewHandler oldHandler)
				oldHandler.NativeViewChanged -= HandleSubviewNativeViewChanged;
		}

		private void HandleSubviewNativeViewChanged(object sender, ViewChangedEventArgs args)
		{
			args.OldNativeView?.RemoveFromSuperview();

			var index = _view.IndexOf(args.VirtualView);
			var newView = args.NewNativeView ?? new UIView();
			InsertSubview(newView, index);
		}

		public virtual void UpdateValue(string property, object value)
		{
			if (property == Gesture.AddGestureProperty)
			{
				ViewHandler.AddGesture(this, (Gesture)value);
			}
			else if (property == Gesture.RemoveGestureProperty)
			{
				ViewHandler.RemoveGesture(this, (Gesture)value);
			}

			Mapper?.UpdateProperty(this, _view, property);
		}

		private void HandleChildrenAdded(object sender, LayoutEventArgs e)
		{
			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				var view = _view[index];

				view.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
				if (view.ViewHandler is iOSViewHandler handler)
					handler.NativeViewChanged += HandleSubviewNativeViewChanged;

				var nativeView = view.ToView() ?? new UIView();
				InsertSubview(nativeView, index);
			}

			SetNeedsLayout();
		}

		private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
		{
			if (e.Removed != null)
			{
				foreach (var view in e.Removed)
				{
					view.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
					if (view.ViewHandler is iOSViewHandler handler)
						handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
				}
			}

			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				var nativeView = Subviews[index];
				nativeView.RemoveFromSuperview();
			}

			SetNeedsLayout();
		}

		private void HandleChildrenChanged(object sender, LayoutEventArgs e)
		{
			if (e.Removed != null)
			{
				foreach (var view in e.Removed)
				{
					view.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
					if (view.ViewHandler is iOSViewHandler handler)
						handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
				}
			}

			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				var oldNativeView = Subviews[index];
				oldNativeView.RemoveFromSuperview();

				var view = _view[index];

				view.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
				if (view.ViewHandler is iOSViewHandler handler)
					handler.NativeViewChanged += HandleSubviewNativeViewChanged;

				var newNativeView = view.ToView() ?? new UIView();
				InsertSubview(newNativeView, index);
			}

			SetNeedsLayout();
		}

		public override CGSize SizeThatFits(CGSize size)
		{
			_measured = _view.GetIntrinsicSize(size.ToSizeF());
			return _measured.ToCGSize();
		}

		public override void SizeToFit()
		{
			var size = Superview?.Bounds.Size;
			if (size == null || ((CGSize)size).IsEmpty)
				size = UIScreen.MainScreen.Bounds.Size;

			_measured = _view.GetIntrinsicSize(((CGSize)size).ToSizeF());
			base.Frame = new CGRect(new CGPoint(0, 0), _measured.ToCGSize());
		}

		public override CGSize IntrinsicContentSize => _measured.ToCGSize();

		public bool IgnoreSafeArea => _view?.GetIgnoreSafeArea(false) ?? false;

		public override void LayoutSubviews()
		{
			if (Superview == null)
				return;

			if (Bounds.Size.IsEmpty)
				return;

			if (_view != null)
			{
				_view.Frame = Frame.ToRectangleF();
				_view.RequestLayout();
			}

		}
	}
}

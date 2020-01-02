using System;
using System.Drawing;
using Android.Runtime;
using Comet.Android.Controls;
using AView = Android.Views.View;
using ARadioGroup = Android.Widget.RadioGroup;
using AViewGroup = Android.Views.ViewGroup;

namespace Comet.Android.Handlers
{
	// TODO: This implementation is basically the same as Comet.Android.Handlers.AbstractLaoutHandler.
	// The primary difference is the base Android type (ViewGroup vs. RadioGroup). 
	// Work out a way to generalize this better.  
	// Since most of the implementation is centered around the interface, AndroidViewhandler,
	// one possibility is to use the C# 8 feature "default interface methods (DIM)"
	// Another option is to encapsulate the base Android type and expose an instance of that instead of 
	// derriving from it.

	class RadioGroupHandler : ARadioGroup, AndroidViewHandler
	{
		private RadioGroup _view;
		private bool _inLayout;

		public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		public RadioGroupHandler() : base(AndroidContext.CurrentContext)
		{

		}

		public RadioGroupHandler(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{

		}

		public AView View => this;

		public CometContainerView ContainerView => null;

		public object NativeView => this;

		public bool HasContainer
		{
			get => false;
			set { }
		}

		public void SetView(View view)
		{
			_view = view as RadioGroup;
			if (_view != null)
			{
				_view.NeedsLayout += HandleNeedsLayout;
				_view.ChildrenChanged += HandleChildrenChanged;
				_view.ChildrenAdded += HandleChildrenAdded;
				_view.ChildrenRemoved += ViewOnChildrenRemoved;

				foreach (var subview in _view)
				{
					subview.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
					if (subview.ViewHandler is AndroidViewHandler handler)
						handler.NativeViewChanged += HandleSubviewNativeViewChanged;

					var nativeView = subview.ToView() ?? new AView(AndroidContext.CurrentContext);
					AddView(nativeView);
				}

				Invalidate();
			}

			ViewHandler.AddGestures(this, view);
		}

		public void Remove(View view)
		{
			ViewHandler.RemoveGestures(this, view);
			foreach (var subview in _view)
			{
				subview.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
				if (subview.ViewHandler is AndroidViewHandler handler)
					handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
			}

			_view.NeedsLayout -= HandleNeedsLayout;
			_view.ChildrenChanged -= HandleChildrenChanged;
			_view.ChildrenAdded -= HandleChildrenAdded;
			_view.ChildrenRemoved -= ViewOnChildrenRemoved;
			_view = null;
		}

		private void HandleNeedsLayout(object sender, EventArgs e) => Invalidate();

		private void HandleSubviewViewHandlerChanged(object sender, ViewHandlerChangedEventArgs e)
		{
			if (e.OldViewHandler is AndroidViewHandler oldHandler)
				oldHandler.NativeViewChanged -= HandleSubviewNativeViewChanged;
		}

		private void HandleSubviewNativeViewChanged(object sender, ViewChangedEventArgs args)
		{
			if (args.OldNativeView != null)
			{
				if (args.OldNativeView.Parent is AViewGroup parent)
					parent.RemoveView(args.OldNativeView);
			}

			var index = _view.IndexOf(args.VirtualView);
			var newView = args.NewNativeView ?? new AView(AndroidContext.CurrentContext);
			AddView(newView, index);
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
		}

		private void HandleChildrenAdded(object sender, LayoutEventArgs e)
		{
			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				var view = _view[index];

				view.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
				if (view.ViewHandler is AndroidViewHandler handler)
					handler.NativeViewChanged += HandleSubviewNativeViewChanged;

				var nativeView = view.ToView() ?? new AView(AndroidContext.CurrentContext);
				AddView(nativeView, index);
			}

			Invalidate();
		}

		private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
		{
			if (e.Removed != null)
			{
				foreach (var view in e.Removed)
				{
					view.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
					if (view.ViewHandler is AndroidViewHandler handler)
						handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
				}
			}

			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				RemoveViewAt(index);
			}

			Invalidate();
		}

		private void HandleChildrenChanged(object sender, LayoutEventArgs e)
		{
			if (e.Removed != null)
			{
				foreach (var view in e.Removed)
				{
					view.ViewHandlerChanged -= HandleSubviewViewHandlerChanged;
					if (view.ViewHandler is AndroidViewHandler handler)
						handler.NativeViewChanged -= HandleSubviewNativeViewChanged;
				}
			}

			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				RemoveViewAt(index);

				var view = _view[index];

				view.ViewHandlerChanged += HandleSubviewViewHandlerChanged;
				if (view.ViewHandler is AndroidViewHandler handler)
					handler.NativeViewChanged += HandleSubviewNativeViewChanged;

				var newNativeView = view.ToView() ?? new AView(AndroidContext.CurrentContext);
				AddView(newNativeView, index);
			}

			Invalidate();
		}

		public CometTouchGestureListener GestureListener { get; set; }

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			if (_view == null) return;

			var width = MeasureSpec.GetSize(widthMeasureSpec);
			var height = MeasureSpec.GetSize(heightMeasureSpec);

			var measured = _view.Measure(new SizeF(width, height));

			// The measured size is in display independent units, so we need to get the display metrics and
			// scale them back to display specific units.
			var density = AndroidContext.DisplayScale;

			var scaledWidth = measured.Width * density;
			var scaledHeight = measured.Height * density;

			SetMeasuredDimension((int)scaledWidth, (int)scaledHeight);
		}

		public SizeF GetIntrinsicSize(SizeF availableSize) => Comet.View.UseAvailableWidthAndHeight;

		public void SetFrame(RectangleF frame)
		{
			if (_inLayout)
				return;

			var scale = AndroidContext.DisplayScale;

			var left = frame.Left * scale;
			var top = frame.Top * scale;
			var bottom = frame.Bottom * scale;
			var right = frame.Right * scale;

			if (LayoutParameters == null)
			{
				LayoutParameters = new AViewGroup.LayoutParams(
					(int)(frame.Width * scale),
					(int)(frame.Height * scale));
			}
			else
			{
				LayoutParameters.Width = (int)(frame.Width * scale);
				LayoutParameters.Height = (int)(frame.Height * scale);
			}

			Layout((int)left, (int)top, (int)right, (int)bottom);
		}

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			if (_view == null || !changed || _inLayout) return;

			var displayScale = AndroidContext.DisplayScale;
			var width = (right - left) / displayScale;
			var height = (bottom - top) / displayScale;

			if (width > 0 && height > 0)
			{
				_inLayout = true;
				var x = left / displayScale;
				var y = top / displayScale;
				_view.Frame = new RectangleF(x, y, width, height);
				_inLayout = false;
			}
		}
	}
}
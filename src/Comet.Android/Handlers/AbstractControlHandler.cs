using System;
using System.Drawing;
using Android.Content;
using Android.Views;
using Comet.Android.Controls;
using AView = Android.Views.View;

namespace Comet.Android.Handlers
{
	public abstract class AbstractControlHandler<TVirtualView, TNativeView> : AndroidViewHandler
		where TVirtualView : View
		where TNativeView : AView
	{
		private PropertyMapper<TVirtualView> _mapper;
		private TVirtualView _virtualView;
		private TNativeView _nativeView;
		private CometContainerView _containerView;

		public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		protected AbstractControlHandler()
		{

		}

		protected AbstractControlHandler(PropertyMapper<TVirtualView> mapper)
		{
			_mapper = mapper;
		}

		protected void SetMapper(PropertyMapper<TVirtualView> mapper)
		{
			_mapper = mapper;
		}

		protected abstract TNativeView CreateView(Context context);

		protected abstract void DisposeView(TNativeView nativeView);

		public AView View => (AView)_containerView ?? _nativeView;

		public CometContainerView ContainerView => _containerView;

		public object NativeView => _nativeView;

		public TNativeView TypedNativeView => _nativeView;

		protected TVirtualView VirtualView => _virtualView;

		public bool HasContainer
		{
			get => _containerView != null;
			set
			{
				if (!value && _containerView != null)
				{
					var oldView = _containerView;
					_containerView.MainView = null;
					_containerView = null;

					NativeViewChanged?.Invoke(this, new ViewChangedEventArgs(VirtualView, oldView, _nativeView));
					return;
				}

				if (value && _containerView == null)
				{
					_containerView = new CometContainerView();
					_containerView.MainView = _nativeView;
					NativeViewChanged?.Invoke(this, new ViewChangedEventArgs(VirtualView, _nativeView, _containerView));
				}
			}
		}

		public CometTouchGestureListener GestureListener { get; set; }

		public virtual SizeF GetIntrinsicSize(SizeF availableSize)
		{
			var scale = AndroidContext.DisplayScale;
			
			var width = AView.MeasureSpec.MakeMeasureSpec((int)(availableSize.Width * scale), MeasureSpecMode.AtMost);
			var height = AView.MeasureSpec.MakeMeasureSpec((int)(availableSize.Height * scale), MeasureSpecMode.AtMost);
			TypedNativeView.Measure(width, height);

			var measuredWidth = TypedNativeView.MeasuredWidth / scale;
			var measuredHeight = TypedNativeView.MeasuredHeight / scale;
			return new SizeF(measuredWidth, measuredHeight);
		}

		public virtual void SetFrame(RectangleF frame)
		{
			var nativeView = TypedNativeView;
			if (nativeView == null) return;

			var scale = AndroidContext.DisplayScale;

			var left = frame.Left * scale;
			var top = frame.Top * scale;
			var bottom = frame.Bottom * scale;
			var right = frame.Right * scale;
            
			if (nativeView.LayoutParameters == null)
			{
				nativeView.LayoutParameters = new ViewGroup.LayoutParams(
					(int) (frame.Width * scale),
					(int) (frame.Height * scale));
			}
			else
			{
				nativeView.LayoutParameters.Width = (int) (frame.Width * scale);
				nativeView.LayoutParameters.Height = (int) (frame.Height * scale);
			}
            
			nativeView.Layout((int)left, (int)top, (int)right, (int)bottom);
		}

		public virtual void Remove(View view)
		{
			ViewHandler.RemoveGestures(this, view);
			_virtualView = null;

			// If a container view is being used, then remove the native view from it and get rid of it.
			if (_containerView != null)
			{
				_containerView.MainView = null;
				_containerView = null;
			}
		}

		public virtual void SetView(View view)
		{
			_virtualView = view as TVirtualView;
			if (_nativeView == null)
				_nativeView = CreateView(AndroidContext.CurrentContext);
			_mapper?.UpdateProperties(this, _virtualView);
			ViewHandler.AddGestures(this, view);
		}

		public virtual void UpdateValue(string property, object value)
		{
			_mapper?.UpdateProperty(this, _virtualView, property);
			if (property == Gesture.AddGestureProperty)
			{
				ViewHandler.AddGesture(this, (Gesture)value);
			}
			else if (property == Gesture.RemoveGestureProperty)
			{
				ViewHandler.RemoveGesture(this, (Gesture)value);
			}
		}

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
				return;

			if (_nativeView != null)
				DisposeView(_nativeView);

			_nativeView?.Dispose();
			_nativeView = null;
			if (_virtualView != null)
				Remove(_virtualView);
		}

		void OnDispose(bool disposing)
		{
			if (disposedValue)
				return;
			disposedValue = true;
			Dispose(disposing);
		}

		~AbstractControlHandler()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			OnDispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			OnDispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}

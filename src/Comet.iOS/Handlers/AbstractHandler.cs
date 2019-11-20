using System;
using Comet.iOS.Controls;
using UIKit;

namespace Comet.iOS.Handlers
{
    public abstract class AbstractHandler<TVirtualView, TNativeView> : iOSViewHandler 
        where TVirtualView : View 
        where TNativeView: UIView
    {
        protected readonly PropertyMapper<TVirtualView> mapper;

        protected AbstractHandler(PropertyMapper<TVirtualView> mapper)
        {
            this.mapper = mapper;
        }

        protected AbstractHandler()
        {
            
        }

        
        private TVirtualView _virtualView;
        private TNativeView _nativeView;

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;
        
        protected abstract TNativeView CreateView();

        public UIView View => _nativeView;
        
        public CUIContainerView ContainerView => null;

        public object NativeView => _nativeView;
        
        public TNativeView TypedNativeView => _nativeView;

        protected TVirtualView VirtualView => _virtualView;

        public virtual void SetView(View view)
        {
            _virtualView = view as TVirtualView;
            _nativeView ??= CreateView();
            mapper?.UpdateProperties(this, _virtualView);
            ViewHandler.AddGestures(this, view);
        }

        public virtual void Remove(View view)
        {
            ViewHandler.RemoveGestures(this, view);
            _virtualView = null;
        }

        protected virtual void DisposeView(TNativeView nativeView)
        {
            
        }

        public virtual void UpdateValue(string property, object value)
        {
            mapper?.UpdateProperty(this, _virtualView, property);
            if (property == Gesture.AddGestureProperty)
            {
                ViewHandler.AddGesture(this, (Gesture)value);
            }
            else if (property == Gesture.RemoveGestureProperty)
            {
                ViewHandler.RemoveGesture(this, (Gesture)value);
            }
        }

        public bool HasContainer
        {
            get => false;
            set { }
        }

        public virtual bool IgnoreSafeArea => VirtualView?.GetIgnoreSafeArea(false) ?? false;

        public virtual SizeF Measure(SizeF availableSize)
        {
            return Comet.View.IllTakeWhatYouCanGive;
        }

        public void SetFrame(RectangleF frame)
        {
            if (_nativeView == null) return;
            _nativeView.Frame = frame.ToCGRect();
        }

        protected void BroadcastNativeViewChanged(UIView previousView, UIView newView)
        {
            NativeViewChanged?.Invoke(this, new ViewChangedEventArgs(VirtualView, previousView, newView));
        }
        
        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls
        
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_nativeView != null)
                DisposeView(_nativeView);
            
            _nativeView?.RemoveFromSuperview();
            _nativeView?.Dispose();
            _nativeView = null;
            
            if (_virtualView != null)
                Remove(_virtualView);
        }

        void OnDispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;
            Dispose(disposing);
        }

        ~AbstractHandler()
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

using System;
using HotUI.iOS.Controls;
using UIKit;

namespace HotUI.iOS.Handlers
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
        
        public HUIContainerView ContainerView => null;

        public object NativeView => _nativeView;
        
        public TNativeView TypedNativeView => _nativeView;

        protected TVirtualView VirtualView => _virtualView;

        public virtual void SetView(View view)
        {
            _virtualView = view as TVirtualView;
            _nativeView = CreateView();
            mapper?.UpdateProperties(this, _virtualView);
        }

        public virtual void Remove(View view)
        {
            _virtualView = null;
            _nativeView = null;
        }

        protected virtual void DisposeView(TNativeView nativeView)
        {
            
        }

        public virtual void UpdateValue(string property, object value)
        {
            mapper?.UpdateProperty(this, _virtualView, property);
        }

        public bool HasContainer
        {
            get => false;
            set { }
        }

        public virtual bool AutoSafeArea => true;

        public virtual SizeF Measure(SizeF availableSize)
        {
            return availableSize;
        }

        public void SetFrame(RectangleF frame)
        {
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
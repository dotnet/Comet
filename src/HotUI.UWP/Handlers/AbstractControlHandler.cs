using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HotUI.UWP.Controls;

namespace HotUI.UWP.Handlers
{
    public abstract class AbstractControlHandler<TVirtualView, TNativeView> : UWPViewHandler
        where TVirtualView : View
        where TNativeView : UIElement
    {
        private readonly PropertyMapper<TVirtualView> _mapper;
        private TVirtualView _virtualView;
        private TNativeView _nativeView;
        private HUIContainerView _containerView;

        public EventHandler ViewChanged { get; set; }

        protected AbstractControlHandler(PropertyMapper<TVirtualView> mapper)
        {
            _mapper = mapper;
        }

        protected abstract TNativeView CreateView();

        protected abstract void DisposeView(TNativeView nativeView);

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        public UIElement View => (UIElement)_containerView ?? _nativeView;

        public HUIContainerView ContainerView => _containerView;

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
                    _containerView.MainView = null; ;
                    _containerView = null;

                    ViewChanged?.Invoke(this, EventArgs.Empty);
                    return;
                }

                if (value && _containerView == null)
                {
                    _containerView = new HUIContainerView();
                    _containerView.MainView = _nativeView;
                    ViewChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public SizeF Measure(SizeF availableSize)
        {
            _nativeView.Measure(availableSize.ToSize());
            return _nativeView.DesiredSize.ToSizeF();
        }

        public void SetFrame(RectangleF frame)
        {
            _nativeView.Arrange(frame.ToRect());
        }

        public virtual void Remove(View view)
        {
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
                _nativeView = CreateView();
            _mapper.UpdateProperties(this, _virtualView);
        }

        public virtual void UpdateValue(string property, object value)
        {
            _mapper.UpdateProperty(this, _virtualView, property);
        }

        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_nativeView is FrameworkElement element)
                element.Parent.RemoveChild(_nativeView);

            if (_nativeView != null)
                DisposeView(_nativeView);

            DisposeView(TypedNativeView);
            if (_nativeView is IDisposable disposableNativeView)
                disposableNativeView?.Dispose();

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

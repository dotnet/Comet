using System;
using HotUI.Mac.Controls;
using AppKit;

namespace HotUI.Mac
{
    public abstract class AbstractHandler<TVirtualView, TNativeView> : MacViewHandler where TVirtualView : View where TNativeView: NSView
    {
        private readonly PropertyMapper<TVirtualView> _mapper;
        private TVirtualView _virtualView;
        private TNativeView _nativeView;
        private HUIContainerView _containerView;
        
        public EventHandler ViewChanged { get; set; }

        protected AbstractHandler(PropertyMapper<TVirtualView> mapper)
        {
            _mapper = mapper;
        }

        protected abstract TNativeView CreateView();
        
        public NSView View => (NSView)_containerView ?? _nativeView;

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
                    _containerView.ShadowLayer = null;
                    _containerView.MaskLayer = null;
                    _containerView = null;

                    _nativeView.RemoveFromSuperview();
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
        
        public virtual void Remove(View view)
        {
            _virtualView = null;

            // If a container view is being used, then remove the native view from it and get rid of it.
            if (_containerView != null)
            {
                _nativeView.RemoveFromSuperview();
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
    }
}

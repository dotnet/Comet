using System;
using HotUI.iOS.Controls;
using UIKit;

namespace HotUI.iOS
{
    public abstract class AbstractHandler<TVirtualView, TNativeView> : iOSViewHandler where TVirtualView : View where TNativeView: UIView
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
        
        public UIView View => (UIView)_containerView ?? _nativeView;

        public HUIContainerView ContainerView => _containerView;
        
        public object NativeView => _nativeView;

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
        
        public void Remove(View view)
        {
            _virtualView = null;

            // If a container view is being used, then remove the native view from it and get rid of it.
            if (_containerView != null)
            {
                _nativeView.RemoveFromSuperview();
                _containerView = null;
            }
        }
        
        public void SetView(View view)
        {
            _virtualView = view as TVirtualView;
            if (_nativeView == null)
                _nativeView = CreateView();
            _mapper.UpdateProperties(this, _virtualView);
        }

        public void UpdateValue(string property, object value)
        {
            _mapper.UpdateProperty(this, _virtualView, property);
        }
    }
}

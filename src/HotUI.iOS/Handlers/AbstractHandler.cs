using System;
using HotUI.iOS.Controls;
using UIKit;

namespace HotUI.iOS
{
    public abstract class AbstractHandler<TVirtualView, TNativeView> : iOSViewHandler 
        where TVirtualView : View 
        where TNativeView: UIView
    {
        private readonly PropertyMapper<TVirtualView> _mapper;
        private TVirtualView _virtualView;
        private TNativeView _nativeView;
        private HUIContainerView _containerView;

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;
        public event EventHandler RemovedFromView;

        protected AbstractHandler(PropertyMapper<TVirtualView> mapper)
        {
            _mapper = mapper;
        }

        protected abstract TNativeView CreateView();
        
        public UIView View => (UIView)_containerView ?? _nativeView;

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
                    var previousContainerView = _containerView;

                    _containerView.ShadowLayer = null;
                    _containerView.MaskLayer = null;
                    _containerView = null;

                    _nativeView.Layer.Mask = null;
                    _nativeView.RemoveFromSuperview();
                    NativeViewChanged?.Invoke(this, new ViewChangedEventArgs(VirtualView, previousContainerView, _nativeView));
                    return;
                }

                if (value && _containerView == null)
                {
                    _containerView = new HUIContainerView();
                    _containerView.MainView = _nativeView;
                    NativeViewChanged?.Invoke(this, new ViewChangedEventArgs(VirtualView, _nativeView, _containerView));
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

            RemovedFromView?.Invoke(this, EventArgs.Empty);
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

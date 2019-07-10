using System;
using Android.Content;
using HotUI.Android.Controls;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public abstract class AbstractHandler<TVirtualView, TNativeView> : AndroidViewHandler 
        where TVirtualView : View 
        where TNativeView: AView
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

        protected abstract TNativeView CreateView(Context context);
        
        public AView View => (AView)_containerView ?? _nativeView;

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
                    _containerView.MainView = null;;
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
                _nativeView = CreateView(AndroidContext.CurrentContext);
            _mapper.UpdateProperties(this, _virtualView);
        }

        public virtual void UpdateValue(string property, object value)
        {
            _mapper.UpdateProperty(this, _virtualView, property);
        }
    }
}

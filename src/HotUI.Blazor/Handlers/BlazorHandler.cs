using HotUI.Blazor.Components;
using System;

namespace HotUI.Blazor.Handlers
{
    internal abstract class BlazorHandler<TVirtualView, TNativeView> : IBlazorViewHandler
      where TVirtualView : View
      where TNativeView : HotUIComponentBase
    {
        private readonly PropertyMapper<TVirtualView> _mapper;

        public BlazorHandler(PropertyMapper<TVirtualView> mapper)
        {
            _mapper = mapper;
        }

        object IViewHandler.NativeView => NativeView;

        public TNativeView NativeView { get; private set; }

        public TVirtualView VirtualView { get; private set; }

        public Type Component => typeof(TNativeView);

        public virtual bool HasContainer { get; set; }

        public virtual SizeF Measure(SizeF availableSize) => availableSize;

        public virtual void Remove(View view)
        {
        }

        public virtual void SetFrame(RectangleF frame)
        {
        }

        public virtual void SetView(View view)
        {
            VirtualView = view as TVirtualView;
            _mapper?.UpdateProperties(this, VirtualView);
        }

        public virtual void UpdateValue(string property, object value)
        {
            NativeView?.NotifyUpdate();
            _mapper?.UpdateProperty(this, VirtualView, property);
        }

        protected virtual void NativeViewUpdated()
        {
            _mapper?.UpdateProperties(this, VirtualView);
        }

        void IBlazorViewHandler.SetNativeView(object nativeView)
        {
            NativeView = (TNativeView)nativeView;
            NativeViewUpdated();
        }
    }
}

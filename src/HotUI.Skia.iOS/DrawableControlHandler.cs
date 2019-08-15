using Comet.iOS.Handlers;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Skia.iOS
{
    public class DrawableControlHandler : AbstractControlHandler<DrawableControl, iOSDrawableControl>
    {
        protected override iOSDrawableControl CreateView()
        {
            return new iOSDrawableControl();
        }

        protected override void DisposeView(iOSDrawableControl nativeView)
        {
            
        }

        public override void SetView(View view)
        {
            base.SetView(view);

            SetMapper(VirtualView.ControlDelegate.Mapper);
            TypedNativeView.ControlDelegate = VirtualView.ControlDelegate;
            VirtualView.ControlDelegate.Mapper?.UpdateProperties(this, VirtualView);
        }

        public override void Remove(View view)
        {
            TypedNativeView.ControlDelegate = null;
            SetMapper(null);
            
            base.Remove(view);
        }

        public override SizeF Measure(SizeF availableSize)
        {
            return VirtualView?.ControlDelegate?.Measure(availableSize) ?? availableSize;
        }
    }
}
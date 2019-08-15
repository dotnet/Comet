using Comet.Mac.Handlers;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Skia.Mac
{
    public class DrawableControlHandler : AbstractControlHandler<DrawableControl, MacDrawableControl>
    {
        protected override MacDrawableControl CreateView()
        {
            return new MacDrawableControl();
        }

        protected override void DisposeView(MacDrawableControl nativeView)
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

using Comet.iOS.Handlers;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Skia.iOS
{
    public class SkiaViewHandler : AbstractControlHandler<SkiaView, iOSSkiaView>
    {
        protected override iOSSkiaView CreateView()
        {
            return new iOSSkiaView();
        }

        protected override void DisposeView(iOSSkiaView nativeView)
        {
        }

        public override void SetView(View view)
        {
            base.SetView(view);

            SetMapper(VirtualView.Mapper);
            TypedNativeView.VirtualView = VirtualView;
            VirtualView.Mapper?.UpdateProperties(this, VirtualView);
        }

        public override void Remove(View view)
        {
            TypedNativeView.VirtualView = null;
            SetMapper(null);
            
            base.Remove(view);
        }

    }
}

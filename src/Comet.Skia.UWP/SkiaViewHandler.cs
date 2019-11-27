using Comet.UWP.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Skia.UWP
{
	public class SkiaViewHandler : AbstractControlHandler<SkiaView, UWPSkiaView>
    {
        protected override UWPSkiaView CreateView()
        {
            return new UWPSkiaView();
        }

        protected override void DisposeView(UWPSkiaView nativeView)
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

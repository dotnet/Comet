using Comet.WPF.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Skia.WPF
{
	public class SkiaViewHandler : AbstractControlHandler<SkiaView, WPFSkiaView>
	{
		protected override WPFSkiaView CreateView()
		{
			return new WPFSkiaView();
		}

		protected override void DisposeView(WPFSkiaView nativeView)
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

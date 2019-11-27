using Comet.UWP.Handlers;
using System.Drawing;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Skia.UWP
{
	public class DrawableControlHandler : AbstractControlHandler<DrawableControl, UWPDrawableControl>
	{
		protected override UWPDrawableControl CreateView()
		{
			return new UWPDrawableControl();
		}

		protected override void DisposeView(UWPDrawableControl nativeView)
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
			ReleaseNativeView();
		}

		public override SizeF Measure(SizeF availableSize)
		{
			return VirtualView?.ControlDelegate?.Measure(availableSize) ?? availableSize;
		}
	}
}

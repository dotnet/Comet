using Comet.Mac.Handlers;
using System.Drawing;

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

		public override SizeF GetIntrinsicSize(SizeF availableSize) => VirtualView?.ControlDelegate?.GetIntrinsicSize(availableSize) ?? availableSize;
	}
}

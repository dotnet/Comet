using Android.Content;
using Comet.Android.Handlers;
using System.Drawing;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Skia.Android
{
	public class DrawableControlHandler : AbstractControlHandler<DrawableControl, AndroidDrawableControl>
	{
		protected override AndroidDrawableControl CreateView(Context context)
		{
			return new AndroidDrawableControl(context);
		}

		protected override void DisposeView(AndroidDrawableControl nativeView)
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

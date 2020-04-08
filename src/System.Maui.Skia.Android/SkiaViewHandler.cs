using Android.Content;
using System.Maui.Android.Handlers;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace System.Maui.Skia.Android
{
	public class SkiaViewHandler : AbstractControlHandler<SkiaView, AndroidSkiaView>
	{
		protected override AndroidSkiaView CreateView(Context context)
		{
			return new AndroidSkiaView(context);
		}

		protected override void DisposeView(AndroidSkiaView nativeView)
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

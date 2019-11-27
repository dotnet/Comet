using Comet.Mac.Handlers;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Skia.Mac
{
	public class SkiaViewHandler : AbstractControlHandler<SkiaView, MacSkiaView>
	{
		protected override MacSkiaView CreateView()
		{
			return new MacSkiaView();
		}

		protected override void DisposeView(MacSkiaView nativeView)
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

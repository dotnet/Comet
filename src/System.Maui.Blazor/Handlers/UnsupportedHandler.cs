using System.Maui.Blazor.Components;

namespace System.Maui.Blazor.Handlers
{
	internal class UnsupportedHandler<TVirtualView> : BlazorHandler<TVirtualView, BUnsupported>
		where TVirtualView : View
	{
		protected override void NativeViewUpdated()
		{
			NativeView.View = VirtualView;
			base.NativeViewUpdated();
		}
	}
}

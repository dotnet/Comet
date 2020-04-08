using Android.Content;

using AView = Android.Views.View;

namespace System.Maui.Android.Handlers
{
	public class SpacerHandler : AbstractControlHandler<Spacer, AView>
	{
		protected override AView CreateView(Context context) => new AView(context);

		protected override void DisposeView(AView nativeView)
		{
			
		}
	}
}

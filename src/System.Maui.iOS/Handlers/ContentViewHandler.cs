using UIKit;

// ReSharper disable ClassNeverInstantiated.Global
namespace System.Maui.iOS.Handlers
{
	public class ContentViewHandler : AbstractHandler<ContentView, UIView>
	{
		protected override UIView CreateView()
		{
			return VirtualView?.Content?.ToView();
		}
		public override bool IgnoreSafeArea => VirtualView?.Content?.GetOrCreateViewHandler().IgnoreSafeArea ?? false;
	}
}

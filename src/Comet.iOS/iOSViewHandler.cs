using System;
using Comet.iOS.Controls;
using UIKit;
namespace Comet.iOS
{
	public interface iOSViewHandler : IViewHandler
	{
		event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		UIView View { get; }

		CUIContainerView ContainerView { get; }

		bool IgnoreSafeArea { get; }
	}
}

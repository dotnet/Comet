using System;
using System.Maui.iOS.Controls;
using UIKit;
namespace System.Maui.iOS
{
	public interface iOSViewHandler : IViewHandler
	{
		event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		UIView View { get; }

		CUIContainerView ContainerView { get; }

		bool IgnoreSafeArea { get; }
	}
}

using System;
using HotUI.iOS.Controls;
using UIKit;
namespace HotUI.iOS 
{
	public interface iOSViewHandler : IViewHandler 
	{
        event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        UIView View { get; }

        HUIContainerView ContainerView { get; }

        bool AutoSafeArea { get; }
    }
}

using System;
using HotUI.iOS.Controls;
using UIKit;
namespace HotUI.iOS 
{
	public interface iOSViewHandler : IViewHandler 
	{
		UIView View { get; }
		HUIContainerView ContainerView { get; }
	}
}

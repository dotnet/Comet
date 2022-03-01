using Comet.iOS;
using Microsoft.Maui.Handlers;
using UIKit;

namespace Comet;

public static partial class HandlerExtensions
{
	public static void AddGesture(this IViewHandler handler, Gesture gesture)
	{
		var nativeView = (UIView)handler.PlatformView;
		nativeView.UserInteractionEnabled = true;
		nativeView.AddGestureRecognizer(gesture.ToGestureRecognizer());
	}

	public static void RemoveGesture(this IViewHandler handler, Gesture gesture)
	{
		var nativeView = (UIView)handler.PlatformView;
		if(gesture.PlatformGesture is UIGestureRecognizer g)
			nativeView.RemoveGestureRecognizer(g);
	}
}



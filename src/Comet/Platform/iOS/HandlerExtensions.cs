using Comet.iOS;
using Microsoft.Maui.Handlers;
using UIKit;

namespace Comet;

public static partial class HandlerExtensions
{
	public static void AddGesture(this IViewHandler handler, Gesture gesture)
	{
		var nativeView = (UIView)handler.NativeView;
		nativeView.UserInteractionEnabled = true;
		nativeView.AddGestureRecognizer(gesture.ToGestureRecognizer());
	}
}



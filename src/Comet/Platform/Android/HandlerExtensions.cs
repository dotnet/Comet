using Microsoft.Maui.Handlers;
using Comet.Android.Controls;
using AView = Android.Views.View;
using MotionEvent = Android.Views.MotionEvent;
using MotionEventActions = Android.Views.MotionEventActions;
namespace Comet;

public static partial class HandlerExtensions
{
	public static void AddGesture(this IViewHandler handler, Gesture gesture)
	{
		var gl = handler.GetGestureListener(true);
		gl.AddGesture(gesture);
	}

	public static void RemoveGesture(this IViewHandler handler, Gesture gesture)
	{
		var gl = handler.GetGestureListener(false);
		gl?.RemoveGesture(gesture);
	}
	public static CometTouchGestureListener GetGestureListener(this IViewHandler handler, bool createIfNull)
	{
		var v = handler.VirtualView as View;
		if (v == null)
			return null;
		var gl = v.GetEnvironment<ObjectWrapper<CometTouchGestureListener>>(nameof(CometTouchGestureListener), false)?.Object;
		if (gl == null && createIfNull)
			v.SetEnvironment(nameof(CometTouchGestureListener), new ObjectWrapper<CometTouchGestureListener> { Object = gl = new CometTouchGestureListener(handler.NativeView as AView, v) }, false);
		return gl;
	}
	public static bool IsComplete(this MotionEvent e)
	{
		switch (e.Action)
		{
			case MotionEventActions.Cancel:
			case MotionEventActions.Outside:
			case MotionEventActions.PointerUp:
			case MotionEventActions.Up:
				return true;
			default:
				return false;
		}
	}

	internal class ObjectWrapper<T>
	{
		public T Object { get; set; }
	}


}



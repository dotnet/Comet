using System;
namespace Comet.Handlers
{
	public partial class CometViewHandler
	{
		public static void AddGesture(IViewHandler viewHandler, IView view, object arg)
		{
			if (arg is not Gesture g)
				return;
			viewHandler.AddGesture(g);
		}
		public static void RemoveGesture(IViewHandler viewHandler, IView view, object arg)
		{
			if (arg is not Gesture g)
				return;
			viewHandler.RemoveGesture(g);
		}
		public static void AddGestures(IViewHandler viewHandler, IView view)
		{
			if (!(view is IGestureView ig))
				return;
			var gestures = ig.Gestures;
			if (!(gestures?.Any() ?? false))
				return;
			foreach (var g in gestures)
				viewHandler.AddGesture(g);
		}
	}
}

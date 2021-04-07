using System;
using System.Reflection;
using Comet.Tests.Handlers;

namespace Comet.Tests
{
	public static class ViewExtensions
	{
		public static View GetReplacedView(this View view)
		{
			var field = typeof(View).GetField("replacedView", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			return (View)field.GetValue(view);
		}

		public static GenericViewHandler SetViewHandlerToGeneric(this View view)
		{
			var handler =  new GenericViewHandler();
			var v = view.ReplacedView;
			view.ViewHandler = handler;
			handler.SetVirtualView(view);
			return handler;
		}
	}
}

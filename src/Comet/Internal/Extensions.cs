using System;
using System.Collections.Generic;
using System.Linq;
using Comet.Reflection;
using Xamarin.Platform;

namespace Comet.Internal
{
	public static class Extensions
	{
		public static View FindViewById(this View view, string id)
			=> View.ActiveViews.FirstOrDefault(x => x.Id == id);

		public static Func<View> GetBody(this View view)
		{
			var bodyMethod = view.GetType().GetDeepMethodInfo(typeof(BodyAttribute));
			if (bodyMethod != null)
				return (Func<View>)Delegate.CreateDelegate(typeof(Func<View>), view, bodyMethod.Name);
			return null;
		}
		public static BindingState InternalGetState(this View view) => view.GetState();
		public static void ResetGlobalEnvironment(this View view) => View.Environment.Clear();

		public static void DisposeAllViews(this View view) => View.ActiveViews.Clear();

		public static View GetView(this View view) => view.GetView();

		//public static Dictionary<Type, Type> GetAllRenderers(this Registrar<View, IViewHandler> registar) => registar.Handler;

		public static T SetParent<T>(this T view, View parent) where T : View
		{
			if (view != null)
				view.Parent = parent;
			return view;
		}

		public static T FindParentOfType<T>(this View view) where T : View
		{
			if (view == null)
				return null;
			if (view.BuiltView is T bt)
			{
				return bt;
			}
			if (view is T t)
				return t;
			return view.Parent?.FindParentOfType<T>();
		}

	}
}

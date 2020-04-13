using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Comet.Reflection;

// ReSharper disable once CheckNamespace
namespace Comet
{
	public static class DatabindingExtensions
	{
		public static void SetBindingValue<T>(this View view, ref Binding<T> currentValue, Binding<T> newValue, [CallerMemberName] string propertyName = "")
		{
			currentValue = newValue ?? new Binding<T>();
			newValue?.BindToProperty(view, propertyName);
		}

		//public static void SetValue<T>(this State state, ref T currentValue, T newValue, View view, [CallerMemberName] string propertyName = "")
		//{
		//    if (state?.IsBuilding ?? false)
		//    {
		//        var props = state.EndProperty();
		//        var propCount = props.Length;
		//        //This is databound!
		//        if (propCount > 0)
		//        {
		//            bool isGlobal = propCount > 1;
		//            if (propCount == 1)
		//            {
		//                var prop = props[0];

		//                var stateValue = state.GetValue(prop).Cast<T>();
		//                var old = state.EndProperty();
		//                //1 to 1 binding!
		//                if (EqualityComparer<T>.Default.Equals(stateValue, newValue))
		//                {
		//                    state.BindingState.AddViewProperty(prop, propertyName, view);
		//                    Debug.WriteLine($"Databinding: {propertyName} to {prop}");
		//                }
		//                else
		//                {
		//                    var errorMessage = $"Warning: {propertyName} is using formated Text. For performance reasons, please switch to a Lambda. i.e new Text(()=> \"Hello\")";
		//                    if (Debugger.IsAttached)
		//                    {
		//                        throw new Exception(errorMessage);
		//                    }

		//                    Debug.WriteLine(errorMessage);
		//                    isGlobal = true;
		//                }
		//            }
		//            else
		//            {
		//                var errorMessage = $"Warning: {propertyName} is using Multiple state Variables. For performance reasons, please switch to a Lambda.";
		//                if (Debugger.IsAttached)
		//                {
		//                    throw new Exception(errorMessage);
		//                }

		//                Debug.WriteLine(errorMessage);
		//            }

		//            if (isGlobal)
		//            {
		//                state.BindingState.AddGlobalProperties(props);
		//            }
		//        }
		//    }

		//    if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
		//        return;
		//    currentValue = newValue;

		//    view.BindingPropertyChanged(propertyName, newValue);
		//}

		public static T Cast<T>(this object val)
		{
			if (val == null)
				return default;
			try
			{
				var type = typeof(T);
				if (val?.GetType().Name == "State`1" && type.Name != "State`1")
				{
					return val.GetPropValue<T>("Value");
				}
				if (type == typeof(string))
				{
					return (T)(object)val?.ToString();
				}

				return (T)val;
			}
			catch
			{
				//This is ok, sometimes the values are not the same.
				return default;
			}
		}


		//public static void SetValue<T>(this View view, State state, ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		//{
		//    if (view.IsDisposed)
		//        return;
		//    state.SetValue<T>(ref currentValue, newValue, view, propertyName);
		//}

		public static View Diff(this View newView, View oldView, bool checkRenderers)
		{
			if (oldView == null)
				return newView;
			var v = newView.DiffUpdate(oldView,checkRenderers);
			//void callUpdateOnView(View view)
			//{
			//    if (view is IContainerView container)
			//    {
			//        foreach (var child in container.GetChildren())
			//        {
			//            callUpdateOnView(child);
			//        }
			//    }
			//    view.FinalizeUpdateFromOldView();
			//};
			//callUpdateOnView(v);
			return v;
		}

		static View DiffUpdate(this View newView, View oldView, bool checkRenderers)
		{
			if (!newView.AreSameType(oldView, checkRenderers))
			{
				return newView;
			}

			//Always diff thebuilt views as well!
			if (newView.BuiltView != null && oldView.BuiltView != null)
			{
				newView.BuiltView.Diff(oldView.BuiltView,checkRenderers);
			}

			if (newView is ContentView ncView && oldView is ContentView ocView)
			{
				ncView.Content?.DiffUpdate(ocView.Content, checkRenderers);
			}
			//Yes if one is IContainer, the other is too!
			else if (newView is IContainerView newContainer && oldView is IContainerView oldContainer)
			{
				var newChildren = newContainer.GetChildren();
				var oldChildren = oldContainer.GetChildren().ToList();
				for (var i = 0; i < Math.Max(newChildren.Count, oldChildren.Count); i++)
				{
					var n = newChildren.GetViewAtIndex(i);
					var o = oldChildren.GetViewAtIndex(i);
					if (n.AreSameType(o, checkRenderers))
					{
						Debug.WriteLine("The controls are the same!");
						DiffUpdate(n, o,checkRenderers);
						continue;
					}

					if (i + 1 >= newChildren.Count || i + 1 >= oldChildren.Count)
					{
						//We are at the end, no point in searching
						continue;
					}

					//Lets see if the next 2 match
					var o1 = oldChildren.GetViewAtIndex(i + 1);
					var n1 = newChildren.GetViewAtIndex(i + 1);
					if (n1.AreSameType(o1, checkRenderers))
					{
						Debug.WriteLine("The controls were replaced!");
						//No big deal the control was replaced!
						continue;
					}

					if (n.AreSameType(o1, checkRenderers))
					{
						//we removed one from the old Children and use the next one

						Debug.WriteLine("One control was removed");
						DiffUpdate(n, o1, checkRenderers);
						oldChildren.RemoveAt(i);
						continue;
					}

					if (n1.AreSameType(o, checkRenderers))
					{
						//The next ones line up, so this was just a new one being inserted!
						//Lets add an empty one to make them line up

						Debug.WriteLine("One control was added");
						DiffUpdate(n1, o,checkRenderers);
						oldChildren.Insert(i, null);
						continue;
					}

					//They don't line up. Maybe we check if 2 were inserted? But for now we are just going to say oh well.
					//The view will jsut be recreated for the restof these!
					Debug.WriteLine("Oh WEll");
					break;
				}
			}

			newView.UpdateFromOldView(oldView);


			return newView;
		}

		static View GetViewAtIndex(this IReadOnlyList<View> list, int index)
		{
			if (index >= list.Count)
				return null;
			return list[index];
		}


		public static bool AreSameType(this View view, View compareView, bool checkRenderers)
		{
			static bool AreSameType(View view, View compareView)
			{
				if (HotReloadHelper.IsReplacedView(view, compareView))
					return true;
				//Add in more edge cases
				var viewView = view?.GetView();
				var compareViewView = compareView?.GetView();

				if (HotReloadHelper.IsReplacedView(viewView, compareViewView))
					return true;

				return viewView?.GetType() == compareViewView?.GetType();
			}
			var areSame = AreSameType(view, compareView);
			if (areSame && checkRenderers && compareView.ViewHandler != null)
			{
				var renderType = Registrar.Handlers.GetRendererType(view.GetType());
				areSame = renderType == compareView.ViewHandler.GetType();
			}
			return areSame;
		}
	}
}

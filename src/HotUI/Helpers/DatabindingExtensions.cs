using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HotUI {
	public static class DatabindingExtensions {
		public static void SetValue<T> (this State state, ref T currentValue, T newValue, Action<string, object> onUpdate, [CallerMemberName] string propertyName = "")
		{
			if (state?.IsBuilding ?? false) {
				var props = state.EndProperty (false);
				var propCount = props.Length;
				//This is databound!
				if (propCount > 0) {
					bool isGlobal = propCount > 1;
					if (propCount == 1) {
						var prop = props [0];

						var stateValue = state.GetValue (prop).Cast<T> ();
						var old = state.EndProperty (false);
						//1 to 1 binding!
						if (EqualityComparer<T>.Default.Equals (stateValue, newValue)) {
							state.BindingState.AddViewProperty (prop,propertyName, onUpdate);
							Debug.WriteLine ($"Databinding: {propertyName} to {prop}");
						} else {
							Debug.WriteLine ($"Warning: {propertyName} is using formated Text. For performance reasons, please switch to TextBinding");
							isGlobal = true;
						}
					} else {
						Debug.WriteLine ($"Warning: {propertyName} is using Multiple state Variables. For performance reasons, please switch to TextBinding");
					}

					if (isGlobal) {

						state.BindingState.AddGlobalProperties (props);
					}
				}
			}
			if (EqualityComparer<T>.Default.Equals (currentValue, newValue))
				return;
			currentValue = newValue;

			onUpdate (propertyName, newValue);
		}
		static T Cast<T> (this object val)
		{
			if (val == null)
				return default;
			try {
				if (typeof (T) == typeof (string)) {
					return (T)(object)val?.ToString ();
				}
				return (T)val;
			} catch (Exception ex) {
				//This is ok, sometimes the values are not the same.
				return default;
			}
		}
		public static void SetValue<T> (this View view, State state, ref T currentValue, T newValue, Action<string, object> onUpdate, [CallerMemberName] string propertyName = "")
		{
			state.SetValue<T> (ref currentValue, newValue, onUpdate, propertyName);
		}

		public static View Diff (this View newView, View oldView)
		{
			var v = newView.DiffUpdate (oldView);
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
		static View DiffUpdate (this View newView, View oldView)
		{
			if (!newView.AreSameType (oldView)) {
				return newView;
			}

			//Yes if one is IContainer, the other is too!
			if (newView is IContainerView newContainer && oldView is IContainerView oldContainer) {
				var newChildren = newContainer.GetChildren ();
				var oldChildren = oldContainer.GetChildren ().ToList ();
				for (var i = 0; i < Math.Max (newChildren.Count, oldChildren.Count); i++) {
					var n = newChildren.GetViewAtIndex (i);
					var o = oldChildren.GetViewAtIndex (i);
					if (n.AreSameType (o)) {
						Debug.WriteLine ("The controls are the same!");
						DiffUpdate (n, o);
						continue;
					}

					if (i + 1 >= newChildren.Count || i + 1 >= oldChildren.Count) {
						//We are at the end, no point in searching
						continue;
					}
					//Lets see if the next 2 match
					var o1 = oldChildren.GetViewAtIndex (i + 1);
					var n1 = newChildren.GetViewAtIndex (i + 1);
					if (n1.AreSameType (o1)) {
						Debug.WriteLine ("The controls were replaced!");
						//No big deal the control was replaced!
						continue;
					}
					if (n.AreSameType (o1)) {
						//we removed one from the old Children and use the next one

						Debug.WriteLine ("One control was removed");
						DiffUpdate (n, o1);
						oldChildren.RemoveAt (i);
						continue;
					}
					if (n1.AreSameType (o)) {
						//The next ones line up, so this was just a new one being inserted!
						//Lets add an empty one to make them line up

						Debug.WriteLine ("One control was added");
						DiffUpdate (n1, o);
						oldChildren.Insert (i, null);
						continue;
					}

					//They don't line up. Maybe we check if 2 were inserted? But for now we are just going to say oh well.
					//The view will jsut be recreated for the restof these!
					Debug.WriteLine ("Oh WEll");
					break;

				}
			}

			newView.UpdateFromOldView (oldView);


			return newView;

		}

		static View GetViewAtIndex (this IReadOnlyList<View> list, int index)
		{
			if (index >= list.Count)
				return null;
			return list [index];
		}



		public static bool AreSameType (this View view, View compareView)
		{

			//Add in more edge cases
			return view?.GetView().GetType () == compareView?.GetView()?.GetType ();
		}


		public static bool SetPropertyValue (this object obj, string name, object value)
		{
			var type = obj.GetType ();
			var info = type.GetProperty (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			if (info != null) {
				info.SetValue (obj, value);
                return true;
			} else {

				var field = type.GetField (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (field == null)
					return false;
				field.SetValue (obj, value);
                return true;
			}
		}

        public static bool SetDeepPropertyValue(this object obj, string name, object value)
        {
            if (obj == null)
                return false;
            var lastObect = obj;
            FieldInfo field = null;
            PropertyInfo info = null;
            foreach (var part in name.Split('.'))
            {
                info = null;
                field = null;
                var type = obj.GetType();
                lastObect = obj;
                info = type.GetDeepProperty(part);
                if (info != null)
                {
                    obj = info.GetValue(obj, null);
                }
                else
                {
                    field = type.GetDeepField(part);
                    if (field == null)
                        return false;
                    obj = field.GetValue(obj);
                }
            }
            if(field != null)
            {
                field.SetValue(lastObect, value);
                return true;
            }
            else if(info != null)
            {
                info.SetValue(lastObect, value);
                return true;
            }
            return false;
        }

        public static FieldInfo GetDeepField (this Type type, string name)
        {
            var  fieldInfo = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (fieldInfo == null && type.BaseType != null)
                    fieldInfo = GetDeepField(type.BaseType, name);
            return fieldInfo;
        }

        public static PropertyInfo GetDeepProperty(this Type type, string name)
        {
            var prop = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null && type.BaseType != null)
                prop = GetDeepProperty(type.BaseType, name);
            return prop;
        }

        public static object GetPropertyValue (this object obj, string name)
		{
			foreach (var part in name.Split ('.')) {
				if (obj == null)
					return null;
				if (obj is BindingObject bo) {
					obj = bo.GetValueInternal (part);
				} else {
					var type = obj.GetType ();
					var info = type.GetProperty (part, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
					if (info != null) {
						obj = info.GetValue (obj, null);
					} else {
						var field = type.GetField (part, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
						if (field == null)
							return null;
						obj = field.GetValue (obj);
					}
				}
			}
			return obj;
		}

		public static T GetPropValue<T> (this object obj, string name)
		{
			var retval = GetPropertyValue (obj, name);
			if (retval == null)
				return default;
			return (T)retval;
		}
	}
}

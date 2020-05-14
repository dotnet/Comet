using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Comet.Helpers;
using Comet.Internal;
using Comet.Reflection;

namespace Comet
{
	public static class StateManager
	{
		static WeakStack<View> currentBuildingView = new WeakStack<View>();
		public static View CurrentView => currentBuildingView.Peek() ?? LastView?.Target as View;
		static WeakReference LastView;
		static Dictionary<string, List<INotifyPropertyRead>> ViewObjectMappings = new Dictionary<string, List<INotifyPropertyRead>>();
		static Dictionary<INotifyPropertyRead, HashSet<View>> NotifyToViewMappings = new Dictionary<INotifyPropertyRead, HashSet<View>>();
		static Dictionary<INotifyPropertyChanged, Dictionary<string, string>> ChildPropertyNamesMapping = new Dictionary<INotifyPropertyChanged, Dictionary<string, string>>();


		static List<INotifyPropertyRead> MonitoredObjects = new List<INotifyPropertyRead>();

		public static List<(INotifyPropertyRead bindingObject, string property)> currentReadProperies = new List<(INotifyPropertyRead bindingObject, string property)>();

		public static bool IsBuilding => isBuilding;
		static bool isBuilding = false;
		public static void ConstructingView(View view)
		{
			LastView = new WeakReference(view);
			// currentBuildingView.Push(view);

			var mappings = CheckForStateAttributes(view, view).ToList();
			if (mappings.Any())
			{
				ViewObjectMappings[view.Id] = mappings;
				foreach (var obj in mappings)
				{
					NotifyToViewMappings.GetOrCreateForKey(obj).Add(view);
				}
			}
			if (currentReadProperies.Any())
			{
				//TODO: Change this to object and property!!!
				CurrentView.GetState().AddGlobalProperties(currentReadProperies);
			}
			currentReadProperies.Clear();

		}

		public static void MonitorListViewObject(View view, INotifyPropertyRead obj)
		{
			ViewObjectMappings.GetOrCreateForKey(view.Id).Add(obj);
			NotifyToViewMappings.GetOrCreateForKey(obj).Add(view);
		}

		public static void Disposing(View view)
		{
			//TODO: Clean up and unsusbscribe from bindings objects
			if (ViewObjectMappings.TryGetValue(view.Id, out var mappings))
			{
				//TODO: clean up mappings
			}

		}

		public static void StartBuilding(View view)
		{
			//TODO: Grab objects and add them to previous views globals
			currentBuildingView.Push(view);
			isBuilding = true;
			if (currentReadProperies.Any())
			{
				//TODO: Change this to object and property!!!
				CurrentView.GetState().AddGlobalProperties(currentReadProperies);
			}
			currentReadProperies.Clear();

		}
		public static void EndBuilding(View view)
		{
			//TODO: Remove from the stack
			var v = currentBuildingView.Pop();
			Debug.Assert(v == view);
			isBuilding = currentBuildingView.Count != 0;
		}


		static Assembly CometAssembly = typeof(BindingObject).Assembly;
		public static void CheckBody(View view)
		{
			CheckForStateAttributes(view, view).ToList();
		}

		static IEnumerable<INotifyPropertyRead> CheckForStateAttributes(object obj, View view)
		{
			//if (hasChecked && obj == this)
			//    return;
			//hasChecked = obj == this;
			var type = obj.GetType();

			var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
				Where(x => (x.FieldType.Assembly == CometAssembly && x.FieldType.Name == "State`1") || Attribute.IsDefined(x, typeof(StateAttribute))).ToList();


			if (fields.Any())
			{
				foreach (var field in fields)
				{
					if (!field.IsInitOnly)
					{
						throw new ReadonlyRequiresException(field.DeclaringType?.FullName, field.Name);
					}
					var fieldValue = field.GetValue(obj);
					var child = fieldValue as INotifyPropertyRead;
					if (child != null)
					{
						//If the view is null, this is a child propety for a binding object.
						//We will need to send its notification out for each view that monitors it.
						RegisterChild(view, child, field.Name);
						yield return child;
					}
				}
			}
		}

		public static void RegisterChild(View view, INotifyPropertyRead value, string fieldName)
		{
			ChildPropertyNamesMapping.GetOrCreateForKey(value)[view?.Id ?? ""] = fieldName;
			if (!MonitoredObjects.Contains(value))
			{
				StartMonitoring(value);
			}
		}

		static public void StartMonitoring(INotifyPropertyRead obj)
		{
			if (MonitoredObjects.Contains(obj))
				return;
			MonitoredObjects.Add(obj);
			//Check in for more properties!
			CheckForStateAttributes(obj, null).ToList();

			//if it is a binding object we auto monitor!!!!
			if (!(obj is BindingObject))
			{
				obj.PropertyChanged += Obj_PropertyChanged;
				obj.PropertyRead += Obj_PropertyRead;
			}
		}
		public static void StopMonitoring(INotifyPropertyRead obj)
		{
			if (!MonitoredObjects.Contains(obj))
				return;
			MonitoredObjects.Remove(obj);
			if (!(obj is BindingObject b))
			{
				obj.PropertyChanged -= Obj_PropertyRead;
				obj.PropertyRead -= Obj_PropertyRead;
			}
			//TODO remove it from all the mappings

		}

		static void Obj_PropertyRead(object sender, PropertyChangedEventArgs e) => OnPropertyRead(sender, e.PropertyName);

		static void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (sender is BindingObject b)
			{
				OnPropertyChanged(sender, e.PropertyName, b.GetValueInternal(e.PropertyName));
				return;
			}

			var value = sender.GetPropertyValue(e.PropertyName);
			OnPropertyChanged(sender, e.PropertyName, value);
		}
		static internal void OnPropertyRead(object sender, string propertyName)
		{
			if (!isBuilding)
				return;
			currentReadProperies.Add((sender as INotifyPropertyRead, propertyName));
		}
		static internal void OnPropertyChanged(object sender, string propertyName, object value)
		{
			if (value?.GetType() == typeof(View))
				return;
			if (value is INotifyPropertyRead iNotify)
				StartMonitoring(iNotify);
			var notify = sender as INotifyPropertyRead;
			if (notify == null)
				throw new Exception("Error, this is null!!!");
			if (!NotifyToViewMappings.TryGetValue(notify, out var views))
			{
				//StopMonitoring(notify);
				return;
			}

			if (!views.Any())
			{
				Console.WriteLine("I think this means it is a child BindingObject");
				return;
			}

			ChildPropertyNamesMapping.TryGetValue(notify, out var mappings);
			List<View> disposedViews = new List<View>();
			views.ForEach((view) => {
				if (view == null || view.IsDisposed)
				{
					disposedViews.Add(view);
					//Cleanup this View
					return;
				}
				string parentproperty = null;
				if (!mappings?.TryGetValue(view.Id, out parentproperty) ?? false && (mappings?.Count ?? 0) > 0)
				{
					parentproperty ??= mappings?.First().Key;
				}
				var prop = string.IsNullOrWhiteSpace(parentproperty) ? propertyName : $"{parentproperty}.{propertyName}";
				//TODO: Change this to use notify and property name
				ThreadHelper.RunOnMainThread(()=>
				view.BindingPropertyChanged(notify, propertyName, prop, value));

				//TODO: Make sure we handle nested binding objects

				/*
                 public class Foo : BindingObject
                 {
                     public Bar Bar {get;set;}
                 }

                 public class Bar : BindingObject
                 {
                     public int Count{get;set;}
                 }

                 //Binding to foo.Bar.Count works when foo.Bar.Count ++;

                */

			});

			//TODO: remove disposedViews;

			//var first = childrenProperty.FirstOrDefault(x => x.Key.Target == sender);
			//string parentproperty = first.Value;
			//var prop = string.IsNullOrWhiteSpace(parentproperty) ? propertyName : $"{parentproperty}.{propertyName}";
			//changeDictionary[prop] = value;
			//pendingUpdates.Add((prop, value));
			//if (!isUpdating)
			//{
			//    EndUpdate();
			//}

		}




		internal static IReadOnlyList<(INotifyPropertyRead BindingObject, string PropertyName)> EndProperty()
		{
			var changed = currentReadProperies.Distinct().ToList();
			currentReadProperies.Clear();
			return changed;

		}


		internal static void StartProperty()
		{
			isBuilding = true;
			if (currentReadProperies.Any())
			{
				//TODO: Change this to object and property!!!
				CurrentView.GetState().AddGlobalProperties(currentReadProperies);
			}
			currentReadProperies.Clear();
		}


		internal static void UpdateBinding(Binding binding, View view)
		{
			foreach (var prop in binding.BoundProperties)
			{
				//ChildPropertyNamesMapping.GetOrCreateForKey(prop.BindingObject).Add(view.Id,)
				NotifyToViewMappings.GetOrCreateForKey(prop.BindingObject).Add(view);
			}
		}

		internal static void ListenToEnvironment(View view)
		{
			NotifyToViewMappings.GetOrCreateForKey(View.Environment).Add(view);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Comet.Reflection;
namespace Comet
{
	public static class HotReloadHelper
	{
		public static void Reset()
		{
			replacedViews.Clear();
		}
		public static bool IsEnabled { get; set; } = Debugger.IsAttached;

		public static void Register(View view, params object[] parameters)
		{
			if (!IsEnabled)
				return;
			currentViews[view] = parameters;
		}

		public static void UnRegister(View view)
		{
			if (!IsEnabled)
				return;
			currentViews.Remove(view);
		}
		public static bool IsReplacedView(View view, View newView)
		{
			if (!IsEnabled)
				return false;
			if (view == null || newView == null)
				return false;

			if (!replacedViews.TryGetValue(view.GetType().FullName, out var newViewType))
				return false;
			return newView.GetType() == newViewType;
		}
		public static View GetReplacedView(View view)
		{
			if (!IsEnabled)
				return view;
			if (!replacedViews.TryGetValue(view.GetType().FullName, out var newViewType))
				return view;

			currentViews.TryGetValue(view, out var parameters);
			try
			{
				var newView = (View)(parameters?.Length > 0 ? Activator.CreateInstance(newViewType, args: parameters) : Activator.CreateInstance(newViewType));
				TransferState(view, newView);
				return newView;
			}
			catch (MissingMethodException ex)
			{
				Debug.WriteLine("You are using Comet.Reload on a view that requires Parameters. Please call `HotReloadHelper.Register(this, params);` in the constructor;");
				throw ex;
			}

		}

		static void TransferState(View oldView, View newView)
		{
			var oldState = oldView.GetState();
			var changes = oldState.ChangedProperties;
			foreach (var change in changes)
			{
				newView.SetDeepPropertyValue(change.Key, change.Value);
			}
		}

		static Dictionary<string, Type> replacedViews = new Dictionary<string, Type>();
		static Dictionary<View, object[]> currentViews = new Dictionary<View, object[]>();
		static Dictionary<string, List<Type>> replacedHandlers = new Dictionary<string, List<Type>>();
		public static void RegisterReplacedView(string oldViewType, Type newViewType)
		{
			if (!IsEnabled || oldViewType == newViewType.FullName)
				return;

			Console.WriteLine($"{oldViewType} - {newViewType}");
			if (newViewType.IsSubclassOf(typeof(View)))
				replacedViews[oldViewType] = newViewType;
			else if (typeof(IViewHandler).IsAssignableFrom(newViewType))
			{

				if (replacedHandlers.TryGetValue(oldViewType, out var vTypes))
				{
					foreach (var vType in vTypes)
						Registrar.Handlers.Register(vType, newViewType);
					return;
				}

				var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
				var t = assemblies.Select(x => x.GetType(oldViewType)).FirstOrDefault(x => x != null);
				var views = Registrar.Handlers.GetViewType(t);
				if (views.Count == 0)
				{
					var staticInit = newViewType.GetMethod("Init", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

					staticInit.Invoke(null, null);
					views = Registrar.Handlers.GetViewType(t);
				}
				replacedHandlers[oldViewType] = views;
				foreach (var h in views)
				{
					Registrar.Handlers.Register(h, newViewType);
				}
			}
		}

		public static async void TriggerReload()
		{
			var roots = View.ActiveViews.Where(x => x.Parent == null).ToList();

			await ThreadHelper.SwitchToMainThreadAsync();
			foreach (var view in roots)
			{
				view.Reload();
			}
		}
	}
}

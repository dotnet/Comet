using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Maui.Devices;
using Comet.Internal;

namespace Comet
{
	public abstract class ContextualObject
	{
		internal static readonly EnvironmentData Environment = new EnvironmentData();
		internal EnvironmentData _context;
		internal EnvironmentData Context(bool shouldCreate) => _context ?? (shouldCreate ? (_context = new EnvironmentData(this)) : null);

		internal EnvironmentData _localContext;
		internal EnvironmentData LocalContext(bool shouldCreate) => _localContext ?? (shouldCreate ? (_localContext = new EnvironmentData(this)) : null);


		protected ContextualObject()
		{

		}

		internal void UpdateBuiltViewContext(View builtView)
		{
			MergeEnvironmentData(_context, builtView.Context(_context != null));
			MergeEnvironmentData(_localContext, builtView.LocalContext(_localContext != null));
		}

		void MergeEnvironmentData(EnvironmentData parent, EnvironmentData child)
		{
			if (parent == null)
				return;
			foreach (var pair in parent.dictionary)
				child.dictionary[pair.Key] = pair.Value;
		}


		internal abstract void ContextPropertyChanged(string property, object value, bool cascades);

		public string StyleId
		{
			get => LocalContext(false)?.GetValueInternal(nameof(StyleId)).value as string;
			set => LocalContext(true).SetPropertyInternal(value);
		}

		public static string GetTypedKey(ContextualObject obj, string key)
			=> GetTypedKey(obj.GetType(), key);
		public static string GetTypedKey(Type type, string key)
			=> type == null ? key : $"{type.Name}.{key}";
		public static string GetTypedStyleId(ContextualObject theObject, string key)
		{
			var styleId = theObject.StyleId;
			return string.IsNullOrWhiteSpace(styleId) ? null : $"{styleId}.{key}";
		}

		public static string GetControlStateKey(ControlState state, string key)
			=> state == ControlState.Default ? key : $"{state}.{key}";

		internal object GetValue(string key, ContextualObject current, View view, string styledKey, string typedKey, bool cascades)
		{
			try
			{
				//Example Environment lookup...
				//Button background color. With a StyleId of 'Foo'
				//key = "BackgroundColor"
				//styledKey = "Foo.BackgroundColor"
				//typedKey = "Button.BackgroundColor"

				//Check the local context
				if (current == this)
				{
					var r = LocalContext(false)?.GetValueInternal(key) ?? (false, null);
					if (r.hasValue)
						return r.value;

					r = LocalContext(false)?.GetValueInternal(styledKey) ?? (false, null);
					if (r.hasValue)
						return r.value;
				}

				if (!cascades)
					return null;
				//Check the cascading context
				//When checking Context, we use the key first, then style, then typed key
				var result = Context(false)?.GetValueInternal(key) ?? (false, null);
				if (result.hasValue)
					return result.value;

				result = Context(false)?.GetValueInternal(styledKey) ?? (false, null);

				if (result.hasValue)
					return result.value;

				result = Context(false)?.GetValueInternal(typedKey) ?? (false, null); ;

				if (result.hasValue)
					return result.value;

				//Check the parent

				//If no more parents, check the environment
				//For global environment check Styled -> Typed -> then root key
				if (view == null)
				{
					result = View.Environment.GetValueInternal(styledKey);

					if (result.hasValue)
						return result.value;
					result = View.Environment.GetValueInternal(typedKey);

					if (result.hasValue)
						return result.value;
					result = View.Environment.GetValueInternal(key);

					if (result.hasValue)
						return result.value;
				}
				return view?.GetValue(key, current, view.Parent, styledKey, typedKey, cascades);
			}
			catch
			{
				return null;
			}
		}
		internal T GetValue<T>(string key, ContextualObject current, View view, string styledKey, string typedKey, bool cascades)
		{
			try
			{
				var value = GetValue(key, current, view, styledKey, typedKey, cascades);
				return value.GetValueOfType<T>() ?? default;
			}
			catch
			{
				return default;
			}
		}

		internal bool SetValue(string key, object value, bool cascades)
		{
			//Monitor changes if we care!
			if (monitoredChanges != null && Thread.CurrentThread == currentMonitoredThread)
			{
				//TODO: Check into this for shapes!!!!!
				var oldValue = this.GetEnvironment(this as View, key, cascades);
				monitoredChanges[(this, key, cascades)] = (oldValue, value);
				return false;
			}

			//We only create the backing dictionary if it is needed. 
			//If we are setting the value to null, 
			//there is no reason to create the dictionary if it doesnt exist
			if (cascades)
				return Context(value != null)?.SetValue(key, value, true) ?? false;
			else
				return LocalContext(value != null)?.SetValue(key, value, false) ?? false;
		}

		static Dictionary<(ContextualObject view, string property, bool cascades), (object oldValue, object newValue)> monitoredChanges = null;
		static Thread currentMonitoredThread;
		static object locker = new object();
		public static void MonitorChanges()
		{
			lock (locker)
			{
				Monitor.Enter(locker);
				monitoredChanges = new Dictionary<(ContextualObject view, string property, bool cascades), (object oldValue, object newValue)>();
				currentMonitoredThread = Thread.CurrentThread;
			}

		}

		public static Dictionary<(ContextualObject view, string property, bool cascades), (object oldValue, object newValue)> StopMonitoringChanges()
		{
			lock (locker)
			{
				var changes = monitoredChanges;
				monitoredChanges = null;
				currentMonitoredThread = null;
				Monitor.Exit(locker);
				return changes;
			}
		}

		//protected ICollection<string> GetAllKeys()
		//{
		//    //This is the global Environment
		//    if (View?.Parent == null)
		//        return dictionary.Keys;

		//    //TODO: we need a fancy way of collapsing this. This may be too slow
		//    var keys = new HashSet<string>();
		//    var localKeys = dictionary?.Keys;
		//    if (localKeys != null)
		//        foreach (var k in localKeys)
		//            keys.Add(k);

		//    var parentKeys = View?.Parent?.Context?.GetAllKeys() ?? View.Environment.GetAllKeys();
		//    if (parentKeys != null)
		//        foreach (var k in parentKeys)
		//            keys.Add(k);
		//    return keys;
		//}
	}

	public static class ContextualObjectExtensions
	{
		public static T SetEnvironment<T>(this T contextualObject, string styleId, string key, object value, bool cascades = true, ControlState state = ControlState.Default)
			where T : ContextualObject
		{
			key = ContextualObject.GetControlStateKey(state, key);
			var typedKey = string.IsNullOrWhiteSpace(styleId) ? key : $"{styleId}.{key}";
			contextualObject.SetValue(typedKey, value, cascades);
			//TODO: Verify this is needed 
			ThreadHelper.RunOnMainThread(() => {
				contextualObject.ContextPropertyChanged(typedKey, value, cascades);
			});
			return contextualObject;
		}

		public static T SetEnvironment<T>(this T contextualObject, Type type, string key, object value, bool cascades = true, ControlState state = ControlState.Default)
			where T : ContextualObject
		{
			key = ContextualObject.GetControlStateKey(state, key);
			var typedKey = ContextualObject.GetTypedKey(type, key);
			contextualObject.SetValue(typedKey, value, cascades);
			//TODO: Verify this is needed 
			ThreadHelper.RunOnMainThread(() => {
				contextualObject.ContextPropertyChanged(typedKey, value, cascades);
			});
			return contextualObject;
		}

		public static T SetEnvironment<T, TValue>(this T view, Type type, string key, Binding<TValue> binding, bool cascades = true, ControlState state = ControlState.Default)
			where T : View
		{
			binding.BindToProperty(view, key);
			key = ContextualObject.GetControlStateKey(state, key);
			var typedKey = ContextualObject.GetTypedKey(type, key);
			view.SetValue(typedKey, binding, cascades);
			//TODO: Verify this is needed 
			ThreadHelper.RunOnMainThread(() => {
				view.ContextPropertyChanged(typedKey, binding, cascades);
			});
			return view;
		}

		public static T SetEnvironment<T, TValue>(this T view, string key, Binding<TValue> binding, bool cascades = true, ControlState state = ControlState.Default)
			where T : View
		{
			binding?.BindToProperty(view, key);
			key = ContextualObject.GetControlStateKey(state, key);
			if (!view.SetValue(key, binding, cascades))
				return view;
			ThreadHelper.RunOnMainThread(() => {
				view.ContextPropertyChanged(key, binding, cascades);
			});
			return view;
		}
		public static T SetEnvironment<T>(this T contextualObject, string key, object value, bool cascades = true, ControlState state = ControlState.Default)
			where T : ContextualObject
		{
			key = ContextualObject.GetControlStateKey(state, key);
			if (!contextualObject.SetValue(key, value, cascades))
				return contextualObject;
			ThreadHelper.RunOnMainThread(() => {
				contextualObject.ContextPropertyChanged(key, value, cascades);
			});
			return contextualObject;
		}

		public static T SetEnvironment<T>(this T contextualObject, string styleId, string key, StyleAwareValue styleValue)
			where T : ContextualObject
		{
			if (styleValue == null)
			{
				contextualObject.SetEnvironment(styleId, key, null, true);
				return contextualObject;
			}
			foreach (var pair in styleValue.ToEnvironmentValues())
			{
				var newKey = pair.key == null ? key : $"{pair.key}.{key}";
				contextualObject.SetEnvironment(styleId, newKey, pair.value);
			}
			return contextualObject;
		}

		public static void SetProperty(this View view, object value, [CallerMemberName] string key = "", bool cascades = true) => view.SetEnvironment(key, value, cascades);
		//public static T SetEnvironment<T>(this T contextualObject, IDictionary<string, object> data, bool cascades = true) where T : ContextualObject
		//{
		//    foreach (var pair in data)
		//        contextualObject.SetValue(pair.Key, pair.Value,cascades);

		//    return contextualObject;
		//}

		public static T GetEnvironment<T>(this ContextualObject contextualObject, View view, string key, bool cascades = true) => contextualObject.GetEnvironment<T>(view, contextualObject.GetType(), key, cascades);
		public static T GetEnvironment<T>(this ContextualObject contextualObject, View view, Type type, string key, bool cascades = true) => contextualObject.GetValue<T>(key, contextualObject, view, ContextualObject.GetTypedStyleId(contextualObject, key), ContextualObject.GetTypedKey(type ?? contextualObject.GetType(), key), cascades);
		public static object GetEnvironment(this ContextualObject contextualObject, View view, string key, bool cascades = true) => contextualObject.GetValue(key, contextualObject, view, ContextualObject.GetTypedStyleId(contextualObject, key), ContextualObject.GetTypedKey(contextualObject, key), cascades);
		public static object GetEnvironment(this ContextualObject contextualObject, View view, Type type, string key, bool cascades = true) => contextualObject.GetValue(key, contextualObject, view, ContextualObject.GetTypedStyleId(contextualObject, key), ContextualObject.GetTypedKey(type ?? contextualObject.GetType(), key), cascades);

		public static T GetEnvironment<T>(this View view, string key, ControlState state, bool cascades = true)
		{
			key = ContextualObject.GetControlStateKey(state, key);
			return view.GetEnvironment<T>(key, cascades);
		}
		public static T GetEnvironment<T>(this View view, string key, bool cascades = true)
			=> view.GetEnvironment<T>(view, view.GetType(), key, cascades);

		public static T GetProperty<T>(this View view, [CallerMemberName] string key = "", bool cascades = true) => view.GetEnvironment<T>(key, cascades);

		public static T GetEnvironment<T>(this View view, Type type, string key, ControlState state, bool cascades = true)
		{
			key = ContextualObject.GetControlStateKey(state, key);
			return view.GetEnvironment<T>(type, key, cascades);
		}

		public static T GetEnvironment<T>(this View view, Type type, string key, bool cascades = true)
			=> view.GetValue<T>(key, view, view.Parent, ContextualObject.GetTypedStyleId(view, key), ContextualObject.GetTypedKey(type ?? view.GetType(), key), cascades);


		public static object GetEnvironment(this View view, string key, bool cascades = true) => view.GetValue(key, view, view.Parent, ContextualObject.GetTypedStyleId(view, key), ContextualObject.GetTypedKey(view, key), cascades);
		public static object GetEnvironment(this View view, Type type, string key, bool cascades = true) => view.GetValue(key, view, view.Parent, ContextualObject.GetTypedStyleId(view, key), ContextualObject.GetTypedKey(type ?? view.GetType(), key), cascades);


		public static Dictionary<string, object> DebugGetEnvironment(this View view)
		{
			var parentDictionary = view.Parent?.DebugGetEnvironment();
			if (parentDictionary == null)
			{
				parentDictionary = new Dictionary<string, object>(ContextualObject.Environment.dictionary);
			}
			if (view._context != null)
				foreach (var pair in view._context.dictionary)
					parentDictionary[pair.Key] = pair.Value;

			if (view._localContext != null)
				foreach (var pair in view._localContext.dictionary)
					parentDictionary[pair.Key] = pair.Value;
			return parentDictionary;
		}
	}
}

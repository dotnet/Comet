using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Comet.Helpers;
using Comet.Reflection;

namespace Comet
{
	public interface IAutoImplemented
	{

	}

	public interface INotifyPropertyRead : INotifyPropertyChanged
	{
		event PropertyChangedEventHandler PropertyRead;
	}
	public class BindingObject : INotifyPropertyRead, IAutoImplemented
	{

		public event PropertyChangedEventHandler PropertyRead;
		public event PropertyChangedEventHandler PropertyChanged;

		internal protected Dictionary<string, object> dictionary = new Dictionary<string, object>();

		protected T GetProperty<T>(T defaultValue = default, [CallerMemberName] string propertyName = "")
		{
			CallPropertyRead(propertyName);

			if (dictionary.TryGetValue(propertyName, out var val))
				return (T)val;
			return defaultValue;
		}

		internal (bool hasValue, object value) GetValueInternal(string propertyName)
		{
			if (string.IsNullOrWhiteSpace(propertyName))
				return (false, null);
			var hasValue = dictionary.TryGetValue(propertyName, out var val);
			return (hasValue, val);
		}
		/// <summary>
		/// Returns true if the value changed
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		protected bool SetProperty<T>(T value, [CallerMemberName] string propertyName = "")
		{
			if (dictionary.TryGetValue(propertyName, out object val))
			{
				if (EqualityComparer<T>.Default.Equals((T)val, value))
					return false;
			}

			dictionary[propertyName] = value;

			CallPropertyChanged(propertyName, value);

			return true;
		}

		protected virtual void CallPropertyChanged(string propertyName, object value)
		{
			StateManager.OnPropertyChanged(this, propertyName, value);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void CallPropertyRead(string propertyName)
		{
			StateManager.OnPropertyRead(this, propertyName);
			PropertyRead?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		internal bool SetPropertyInternal(object value, [CallerMemberName] string propertyName = "")
		{
			dictionary[propertyName] = value;
			CallPropertyChanged(propertyName, value);

			return true;
		}

	}


	public class BindingState
	{
		public IEnumerable<KeyValuePair<string, object>> ChangedProperties => changeDictionary;
		Dictionary<string, object> changeDictionary = new Dictionary<string, object>();

		public HashSet<(INotifyPropertyRead BindingObject, string PropertyName)> GlobalProperties { get; set; } = new HashSet<(INotifyPropertyRead BindingObject, string PropertyName)>();
		public Dictionary<(INotifyPropertyRead BindingObject, string PropertyName), HashSet<(string PropertyName, Binding Binding)>> ViewUpdateProperties = new Dictionary<(INotifyPropertyRead BindingObject, string PropertyName), HashSet<(string PropertyName, Binding Binding)>>();
		public void AddGlobalProperty((INotifyPropertyRead BindingObject, string PropertyName) property)
		{
			if (GlobalProperties.Add(property))
				Debug.WriteLine($"Adding Global Property: {property}");
		}
		public void AddGlobalProperties(IReadOnlyList<(INotifyPropertyRead BindingObject, string PropertyName)> properties)
		{
			var props = properties.ToList();
			foreach (var prop in props)
				AddGlobalProperty(prop);
		}
		public void AddViewProperty((INotifyPropertyRead BindingObject, string PropertyName) property, string propertyName, Binding binding)
		{
			if (!ViewUpdateProperties.TryGetValue(property, out var actions))
				ViewUpdateProperties[property] = actions = new HashSet<(string PropertyName, Binding Binding)>();
			actions.Add((propertyName, binding));
		}

		public void AddViewProperty(IReadOnlyList<(INotifyPropertyRead BindingObject, string PropertyName)> properties, Binding binding, string propertyName)
		{
			foreach (var p in properties)
			{
				AddViewProperty(p, propertyName ?? p.PropertyName, binding);
			}
		}
		public void Clear()
		{
			GlobalProperties?.Clear();
			foreach (var key in ViewUpdateProperties)
			{
				key.Value.Clear();
			}
			ViewUpdateProperties.Clear();
		}

		protected void UpdatePropertyChangeProperty(View view, string fullProperty, object value)
		{
			if (view.Parent != null)
				UpdatePropertyChangeProperty(view.Parent, fullProperty, value);
			else
				view.GetState().changeDictionary[fullProperty] = value;
		}
		public bool UpdateValue(View view,(INotifyPropertyRead BindingObject, string PropertyName) property, string fullProperty, object value)
		{
			changeDictionary[fullProperty] = value;
			UpdatePropertyChangeProperty(view, fullProperty, value);
			if (ViewUpdateProperties.TryGetValue((property.BindingObject, property.PropertyName), out var bindings))
			{
				foreach (var binding in bindings.ToList())
				{
					binding.Binding.BindingValueChanged(property.BindingObject, binding.PropertyName, value);
				}
			}
			if (GlobalProperties.Contains(property))
			{
				return false;
			}
			return true;
		}
	}
}

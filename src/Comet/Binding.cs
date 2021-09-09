using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Comet.Reflection;

namespace Comet
{
	public class Binding
	{
		public object Value { get; protected set; }

		WeakReference _view;
		internal View View
		{
			get => _view?.Target as View;
			set => _view = new WeakReference(value);
		}
		WeakReference _boundFromView;
		internal View BoundFromView
		{
			get => _boundFromView?.Target as View;
			set => _boundFromView = new WeakReference(value);
		}

		public IReadOnlyList<(INotifyPropertyChanged BindingObject, string PropertyName)> BoundProperties { get; protected set; }
		protected string PropertyName;
		public virtual void BindingValueChanged(INotifyPropertyChanged bindingObject, string propertyName, object value)
		{
			Value = value;
			View.ViewPropertyChanged(propertyName, value);
		}

	}

	public class Binding<T> : Binding
	{
		public Binding()
		{

		}
		public Binding(Expression<Func<T>> getValue, Action<T> setValue)
		{
			Func<T> func = getValue.Compile();
			var visitor = new PropertyExpressionVisitor(false);
			visitor.Visit(getValue);
			var bindings = visitor.GetBoundProperties();
			var result = func.Invoke();
			CurrentValue = result;
			Get = func;
			Set = setValue;
		}

		Func<T> Get { get; set; }
		Action<T> _set;
		public Action<T> Set
		{
			get => _set ?? (_set = (v)=>CurrentValue = v);
			internal set => _set = value;
		}

		public T CurrentValue { get => Value == null ? default :  (T)Value; private set => Value = value; }


		public static implicit operator Binding<T>(Expression<Func<T>> value) => 
			new Binding<T>(
				getValue: value,
				setValue: null);

		public static implicit operator T(Binding<T> value)
			=> value == null
			? default : value.CurrentValue;


		public void BindToProperty(View view, string property)
		{
			PropertyName = property;
			View = view;
			if (BoundProperties?.Count > 0)
			{
				StateManager.UpdateBinding(this, view);
			}
		}
		public override void BindingValueChanged(INotifyPropertyChanged bindingObject, string propertyName, object value)
		{
			var oldValue = CurrentValue;
			var result = Get == null ? default : Get.Invoke();
			CurrentValue = result;
			

		}

		static T Cast(object value)
		{
			if (value is T v)
				return v;
			if (typeof(T) == typeof(string))
				return (T)(object)value?.ToString();
			var error = new InvalidCastException()
			{
				Data =
				{
					["Value"] = value,
					["T Type"] = typeof(T),
					["Value Type"] = value?.GetType(),
				}
			};
			Logger.Error(error, typeof(T), value);
			throw error;
		}
	}

	public static class BindingExtensions
	{
		public static T GetValueOrDefault<T>(this Binding<T> binding, T defaultValue = default)
		{
			if (binding.Value == null)
				return defaultValue;
			return binding.CurrentValue;
		}
	}
}

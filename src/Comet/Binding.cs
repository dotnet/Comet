using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Comet.Reflection;

namespace Comet
{
	public class Binding
	{
		public object Value { get; set; }

		public bool IsValue { get; internal set; }
		public bool IsFunc { get; internal set; }
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

		public IReadOnlyList<(INotifyPropertyRead BindingObject, string PropertyName)> BoundProperties { get; protected set; }
		protected string PropertyName;
		public virtual void BindingValueChanged(INotifyPropertyRead bindingObject, string propertyName, object value)
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
		public Binding(Func<T> getValue, Action<T> setValue)
		{
			Get = getValue;
			ProcessGetFunc();
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

		public static implicit operator Binding<T>(T value)
		{
			var props = StateManager.EndProperty();
			if (props?.Count > 1)
			{
				StateManager.CurrentView.GetState().AddGlobalProperties(props);
			}

			else if (props?.Count == 1 && props[0].BindingObject is State<T> state)
			{
				return state;
			}
			return new Binding<T>()
			{
				IsValue = true,
				CurrentValue = value,
				BoundProperties = props,
				BoundFromView = StateManager.CurrentView
			};
		}

		public static implicit operator Binding<T>(Func<T> value)
			=> new Binding<T>(
				getValue: value,
				setValue: null);
		protected void ProcessGetFunc()
		{
			StateManager.StartProperty();
			var result = Get == null ? default : Get.Invoke();
			var props = StateManager.EndProperty();
			IsFunc = true;
			CurrentValue = result;
			BoundProperties = props;

			BoundFromView = StateManager.CurrentView;

		}


		public static implicit operator Binding<T>(State<T> state)
		{
			StateManager.StartProperty();
			var result = state.Value;
			var props = StateManager.EndProperty();


			var binding = new Binding<T>(
				getValue: () => state.Value,
				setValue: (v) => {
					state.Value = v;
				})
			{
				CurrentValue = result,
				BoundProperties = props,
				IsFunc = true,
			};
			return binding;
		}

		public static implicit operator T(Binding<T> value)
			=> value == null
			? default : value.CurrentValue;



		private static Func<object> ToGenericGetter(Func<T> getValue)
		{
			if (getValue != null)
				return () => getValue.Invoke();

			return null;
		}

		private static Action<object> ToGenericSetter(Action<T> setValue)
		{
			if (setValue != null)
				return (v) => setValue.Invoke((T)v);

			return null;
		}

		public void BindToProperty(View view, string property)
		{
			PropertyName = property;
			View = view;
			if (IsFunc && BoundProperties?.Count > 0)
			{
				StateManager.UpdateBinding(this, view);
				view.GetState().AddViewProperty(BoundProperties, this, property);
				return;
			}

			if (IsValue)
			{

				bool isGlobal = BoundProperties?.Count > 1;
				var propCount = BoundProperties?.Count ?? 0;
				if (propCount == 0)
					return;

				var prop = BoundProperties[0];
				if (BoundProperties?.Count == 1)
				{


					var stateValue = prop.BindingObject.GetPropertyValue(prop.PropertyName).Cast<T>();
					var old = StateManager.EndProperty();
					//1 to 1 binding!
					if (EqualityComparer<T>.Default.Equals(stateValue, CurrentValue))
					{
						Set = (v) => {
							prop.BindingObject.SetPropertyValue(prop.PropertyName, v);
							CurrentValue = v;
							//view?.BindingPropertyChanged(property, v);
						};
						StateManager.UpdateBinding(this, view);
						view.GetState().AddViewProperty(prop, property, this);
						Debug.WriteLine($"Databinding: {property} to {prop}");
					}
					else
					{
						var errorMessage = $"Warning: {property} is using formated Text. For performance reasons, please switch to a Lambda. i.e new Text(()=> \"Hello\")";
						if (Debugger.IsAttached)
						{
							Logger.Fatal(errorMessage);
						}

						Debug.WriteLine(errorMessage);
						isGlobal = true;
					}
				}
				else
				{
					var errorMessage = $"Warning: {property} is using Multiple state Variables. For performance reasons, please switch to a Lambda.";
					//if (Debugger.IsAttached)
					//{
					//    throw new Exception(errorMessage);
					//}
					isGlobal = true;
					Debug.WriteLine(errorMessage);
				}

				if (isGlobal)
				{
					StateManager.UpdateBinding(this, BoundFromView);
					BoundFromView.GetState().AddGlobalProperties(BoundProperties);
				}
				else
				{
					StateManager.UpdateBinding(this, BoundFromView);
				}
			}
		}
		public override void BindingValueChanged(INotifyPropertyRead bindingObject, string propertyName, object value)
		{
			var oldValue = CurrentValue;
			if (IsFunc)
			{
				var oldProps = BoundProperties;
				StateManager.StartProperty();
				var result = Get == null ? default : Get.Invoke();
				var props = StateManager.EndProperty();
				CurrentValue = result;
				BoundProperties = props;
				if(BoundProperties.Except(oldProps).Any())
					BindToProperty(View, PropertyName);
			}
			else
			{
				CurrentValue = Cast(value);
			}
			if(!(oldValue?.Equals(CurrentValue) ?? false))
				View?.ViewPropertyChanged(propertyName, value);

		}
		T Cast(object value)
		{
			if (value is T v)
				return v;
			if (typeof(T) == typeof(string))
				return (T)(object)value?.ToString();
			throw new InvalidCastException();
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

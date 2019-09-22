using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using Comet.Reflection;

namespace Comet
{
    public class Binding
    {
        public Binding(Func<object> getValue, Action<object> setValue)
        {
            GetValue = getValue;
            SetValue = setValue;
        }

        public Func<object> GetValue { get; }
        public Action<object> SetValue { get; }

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

        internal void SetInternalValue(string key, object value)
        {
            //View.SetValue()
            Console.Write("Test");
        }

        public IReadOnlyList<(INotifyPropertyRead BindingObject, string PropertyName)> BoundProperties { get; protected set; }

    }

    public class Binding<T> : Binding
    {
        public Binding(Func<T> getValue, Action<T> setValue)
            : base(ToGenericGetter(getValue), ToGenericSetter(setValue))
        {
            Get = getValue;
            Set = setValue;
        }

        public Func<T> Get { get; private set; }
        public Action<T> Set
        {
            get;
            private set;
        }
        public T CurrentValue { get; private set; }
        
        public static implicit operator Binding<T>(T value)
        {
            var props = StateManager.EndProperty();
            if(props?.Count > 1)
            {
                StateManager.CurrentView.GetState().AddGlobalProperties(props);
            }
            return new Binding<T>(
                getValue: () => value,
                setValue: null)
            {
                IsValue = true,
                CurrentValue = value,
                BoundProperties = props,
                BoundFromView = StateManager.CurrentView
            };
        }

        public static implicit operator Binding<T>(Func<T> value)
        {
            StateManager.StartProperty();
            var result = value == null ? default : value.Invoke();
            var props = StateManager.EndProperty();
            return new Binding<T>(
                getValue: value,
                setValue: null)
            {
                IsFunc = true,
                CurrentValue = result,
                BoundProperties = props,
                BoundFromView = StateManager.CurrentView
            };
        }


        public static implicit operator Binding<T>(State<T> state)
        {
            StateManager.StartProperty();
            var result = state.Value;
            var props = StateManager.EndProperty();


            var binding = new Binding<T>(
                getValue: () => state.Value,
                setValue: (v) =>
                {
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
            => value == null || value.Get == null
            ? default : value.Get.Invoke();



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
            if (IsFunc && BoundProperties?.Count > 0)
            {
                StateManager.UpdateBinding(this, BoundFromView);
                view.GetState().AddViewProperty(BoundProperties, view, property);
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
                    var newValue = Get.Invoke();
                    var old = StateManager.EndProperty();
                    //1 to 1 binding!
                    if (EqualityComparer<T>.Default.Equals(stateValue, newValue))
                    {
                        Get = () => view.GetPropertyValue(prop.PropertyName).Cast<T>();
                        Set = (v) =>
                        {
                            view?.SetDeepPropertyValue(property, v);
                            view?.BindingPropertyChanged(property, v);
                        };
                        CurrentValue = newValue;
                        IsValue = false;
                        IsFunc = true;
                        StateManager.UpdateBinding(this, view);
                        view.GetState().AddViewProperty(prop, property, view);
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
                    if (Debugger.IsAttached)
                    {
                        throw new Exception(errorMessage);
                    }

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
    }

    public static class BindingExtensions
    {
        public static T GetValueOrDefault<T>(this Binding<T> binding, T defaultValue = default)
        {
            if (binding?.Get == null)
                return defaultValue;

            return binding.Get.Invoke();
        }

        internal static Binding<T> TwoWayBinding<TBindingObject, T>(this TBindingObject binding, Expression<Func<TBindingObject, T>> expression) where TBindingObject : BindingObject
        {
            if (expression.Body is MemberExpression member)
            {
                var memberName = member.Member.Name;
                var getValue = expression.Compile();

                return new Binding<T>(
                    getValue: () => getValue.Invoke(binding),
                    setValue: value =>
                    {
                        binding.SetPropertyInternal(value, memberName);
                    });
            }
            else
            {
                var getValue = expression.Compile();

                return new Binding<T>(
                    getValue: () => getValue.Invoke(binding),
                    setValue: null)
                { IsFunc = true };
            }
        }
    }
}

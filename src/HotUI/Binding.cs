using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace HotUI
{
    public class Binding
    {
        public Binding(Func<object> getValue, Action<object> setValue)
        {
            GetValue = getValue;
            SetValue = setValue;
        }
        
        public Func<object> GetValue { get; }
        public Action<object> SetValue { get;  }
        
        public bool IsValue { get; internal set; }
        public bool IsFunc { get; internal set; }
    }
    
    public class Binding<T> : Binding
    {
        public Binding(Func<T> getValue, Action<T> setValue) 
            : base(ToGenericGetter(getValue),ToGenericSetter(setValue))
        {
            Get = getValue;
            Set = setValue;
        }

        public Func<T> Get { get; private set; }
        public Action<T> Set {
            get;
            private set;
        }
        public T CurrentValue { get; private set; }

        WeakReference _view;
        internal View View
        {
            get => _view?.Target as View;
            set => _view = new WeakReference(value);
        }

        public string[] BoundProperties { get; private set; }

        public static implicit operator Binding<T>(T value)
        {
            var state = StateBuilder.CurrentState;
            var props = state?.EndProperty();
            return new Binding<T>(
                getValue: () => value,
                setValue: null) {
                IsValue = true,
                CurrentValue = value,
                BoundProperties = props,
            };
        }


        public static implicit operator Binding<T>(Func<T> value)
        {
            var state = StateBuilder.CurrentState;
            state?.StartProperty();
            var result = value.Invoke();
            var props = state?.EndProperty();
            return new Binding<T>(
                getValue: value,
                setValue: null) {
                IsFunc = true,
                CurrentValue = result,
                BoundProperties = props,
            };
        }


        public static implicit operator Binding<T>(State<T> state)
        {

            var bindingState = StateBuilder.CurrentState;
            bindingState?.StartProperty();
            var result = state.Value;
            var props = bindingState?.EndProperty();


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

        public void BindToProperty(State state, View view, string property)
        {
            if(IsFunc && BoundProperties?.Length > 0)
            {
                state.BindingState.AddViewProperty(BoundProperties, view, property);
                return;
            }

            if(IsValue)
            {

                bool isGlobal = BoundProperties?.Length > 1;
                var propCount = BoundProperties?.Length ?? 0;
                if (propCount == 0)
                    return;

                var prop = BoundProperties[0];
                if (BoundProperties?.Length == 1)
                {


                    var stateValue = state.GetValue(prop).Cast<T>();
                    var newValue = Get.Invoke();
                    var old = state.EndProperty(false);
                    //1 to 1 binding!
                    if (EqualityComparer<T>.Default.Equals(stateValue, newValue))
                    {
                        Get = () => state.GetValue(prop).Cast<T>();
                        Set = (v) => state.SetChildrenValue(property, v);
                        CurrentValue = newValue;
                        IsValue = false;
                        IsFunc = true;
                        state.BindingState.AddViewProperty(prop, property, view);
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
                    state.BindingState.AddGlobalProperties(BoundProperties);
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
        
        internal static Binding<T> TwoWayBinding<TBindingObject, T>(this TBindingObject binding, Expression<Func<TBindingObject, T>> expression) where TBindingObject:BindingObject
        {
            if (expression.Body is MemberExpression member)
            {
                var memberName = member.Member.Name;
                var getValue = expression.Compile();

                return new Binding<T>(
                    getValue: () => getValue.Invoke(binding),
                    setValue: value => binding.SetPropertyInternal(value, memberName));
            }
            else
            {
                var getValue = expression.Compile();

                return new Binding<T>(
                    getValue: () => getValue.Invoke(binding),
                    setValue: null) { IsFunc = true };
            }
        }
    }
}
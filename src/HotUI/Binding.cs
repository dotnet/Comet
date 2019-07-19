using System;
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

        public Func<T> Get { get; }
        public Action<T> Set { get; }
        
        public static implicit operator Binding<T>(T value)
        {
            return new Binding<T>(
                getValue: () => value,
                setValue: null) { IsValue = true };
        }

        public static implicit operator Binding<T>(Func<T> value)
        {
            return new Binding<T>(
                getValue: value,
                setValue: null) {IsFunc = true};
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
    }

    public static class BindingExtensions
    {
        public static T GetValueOrDefault<T>(this Binding<T> binding, T defaultValue = default)
        {
            if (binding?.Get == null)
                return defaultValue;

            return binding.Get.Invoke();
        }
        
        public static Binding<T> BindingFor<TBindingObject, T>(this TBindingObject binding, Expression<Func<TBindingObject, T>> expression) where TBindingObject:BindingObject
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
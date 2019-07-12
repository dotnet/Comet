using System;

namespace HotUI
{
    public class Binding<T>
    {
        public Binding(Func<T> getValue, Action<T> setValue)
        {
            Get = getValue;
            Set = setValue;
        }
        
        public Func<T> Get { get; }
        public Action<T> Set { get; }
        
        public static implicit operator Binding<T>(T value) => new Binding<T>(
            getValue:() => value,
            setValue: null);
        
        public static implicit operator Binding<T>(Func<T> value) => new Binding<T>(
            getValue: value,
            setValue: null);
    }

    public static class BindingExtensions
    {
        public static T GetValueOrDefault<T>(this Binding<T> binding, T defaultValue = default)
        {
            if (binding?.Get == null)
                return defaultValue;

            return binding.Get.Invoke();
        }
    }
}
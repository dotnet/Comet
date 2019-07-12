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
        
        public bool IsValue { get; private set; }
        public bool IsFunc { get; private set; }

        public static implicit operator Binding<T>(T value) => new Binding<T>(
            getValue:() => value,
            setValue: null) { IsValue = true };
        
        public static implicit operator Binding<T>(Func<T> value) => new Binding<T>(
            getValue: value,
            setValue: null) { IsFunc = true };
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
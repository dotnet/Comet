using System;

namespace HotUI
{
    public class Binding<T> : IBinding
    {
        public Binding(Func<T> getValue, Action<T> setValue)
        {
            Get = getValue;
            Set = setValue;
        }
        
        public Func<T> Get { get; }
        public Action<T> Set { get; }
        
        public bool ImplicitFromValue { get; set; }
        
        public static implicit operator Binding<T>(T value) => new Binding<T>(
            getValue:() => value,
            setValue: null) { ImplicitFromValue = true };
    }
}
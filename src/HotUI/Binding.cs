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
        public bool Implicit { get; set; }
        
        public static implicit operator Binding<T>(T value) => new Binding<T>(
            getValue:() => value,
            setValue: null) { Implicit = true };
    }
}
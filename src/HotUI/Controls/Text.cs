using System;

namespace HotUI
{
    /// <summary>
    /// A view that displays one or more lines of read-only text.
    /// </summary>
    public class Text : BoundControl<string>
    {
        public Text (
            Binding<string> value = null) : base(value, nameof(Value))
        {

        }
        
        public Text (
            Func<string> value) : this((Binding<string>)value)
        {

        }
        
        public string Value
        {
            get => BoundValue;
            private set => BoundValue = value;
        }
    }
}
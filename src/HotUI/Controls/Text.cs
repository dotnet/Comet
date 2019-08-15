using System;

namespace Comet
{
    /// <summary>
    /// A view that displays one or more lines of read-only text.
    /// </summary>
    public class Text : View
    {
        public Text (
            Binding<string> value = null)
        {
            Value = value;
        }
        public Text(
            Func<string> value) : this((Binding<string>)value)
        {

        }

            Binding<string> _value;
        public Binding<string> Value
        {
            get => _value;
            private set => this.SetBindingValue(ref _value, value);
        }
    }
}
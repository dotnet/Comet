using System;

namespace HotUI
{

    public interface ITextView
    {
        string TextValue { get; }
    }

    /// <summary>
    /// A view that displays one or more lines of read-only text.
    /// </summary>
    public class Text : BoundControl<string>, ITextView
    {
        public Text (
            Binding<string> value = null) : base(value, nameof(TextValue))
        {

        }
        
        public Text (
            Func<string> value) : this((Binding<string>)value)
        {

        }

        public string TextValue => BoundValue;
    }
}
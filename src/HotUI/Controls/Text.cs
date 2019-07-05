using System;

namespace HotUI
{
    /// <summary>
    /// A view that displays one or more lines of read-only text.
    /// </summary>
    public class Text : View
    {
        public Text()
        {
        }

        public Text(string value) : base(true)
        {
            Value = value;
        }

        public Text(Func<string> formattedText)
        {
            TextBinding = formattedText;
        }

        private string _value;

        public string Value
        {
            get => _value;
            private set => this.SetValue(State, ref _value, value, ViewPropertyChanged);
        }

        public Func<string> TextBinding { get; }

        protected override void WillUpdateView()
        {
            base.WillUpdateView();
            if (TextBinding != null)
            {
                State.StartProperty();
                var text = TextBinding.Invoke();
                var props = State.EndProperty();
                var propCount = props.Length;
                if (propCount > 0)
                {
                    State.BindingState.AddViewProperty(props, (s, o) => Value = TextBinding.Invoke());
                }

                Value = text;
            }
        }
    }
}
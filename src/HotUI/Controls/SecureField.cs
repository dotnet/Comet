using System;

namespace HotUI
{
	public class SecureField : BoundView<string> 
	{
        public SecureField(
            Binding<string> value = null,
            string placeholder = null,
            Action<string> onCommit = null) : base(value, nameof(Text))
        {
            Placeholder = placeholder;
            OnCommit = onCommit;
        }

        public SecureField(
            Func<string> value = null,
            string placeholder = null,
            Action<string> onCommit = null) : this((Binding<string>)value, placeholder, onCommit)
        {

        }

		public string Text {
			get => BoundValue;
			private set => BoundValue = value;
		}
		
		string placeholder;
		public string Placeholder {
			get => placeholder;
			set => this.SetValue (State, ref placeholder, value, ViewPropertyChanged);
		}
		
		public Action<string> OnCommit { get; set; }
	}
}

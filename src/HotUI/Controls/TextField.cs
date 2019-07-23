using System;

namespace HotUI 
{
	public class TextField : BoundControl<string>, ITextView
	{
		public TextField (
			Binding<string> value = null, 	
			string placeholder = null,
			Action<string> onEditingChanged = null,
			Action<string> onCommit = null) : base(value, nameof(Text))
		{
			Placeholder = placeholder;
			OnEditingChanged = new MulticastAction<string>(value, onEditingChanged);
			OnCommit = onCommit;
		}

		public TextField(
			Func<string> value = null,
			string placeholder = null,
			Action<string> onEditingChanged = null,
			Action<string> onCommit = null) : this((Binding<string>)value, placeholder, onEditingChanged, onCommit)
		{

		}

		string placeholder;
		public string Placeholder {
			get => placeholder;
			set => SetValue ( ref placeholder, value);
		}

		public Action<TextField> Focused { get; private set; }
		public Action<TextField> Unfocused { get; private set; }
		public Action<string> OnEditingChanged { get; private set; }
		public Action<string> OnCommit { get; private set; }

        public string TextValue => BoundValue;
    }
}

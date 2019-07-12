using System;

namespace HotUI 
{
	public class TextField : View 
	{
		public TextField (string placeholder, Action<string> onEditingChanged = null, Action<string> onCommit = null) : base (true)
		{
			Placeholder = placeholder;
			OnEditingChanged = onEditingChanged;
			OnCommit = onCommit;
		}
		
		public TextField (string text = null, string placeholder = null, Action<string> onEditingChanged = null, Action<string> onCommit = null) : base (true)
		{
			Text = text;
			Placeholder = placeholder;
			OnEditingChanged = onEditingChanged;
			OnCommit = onCommit;
		}
		
		public TextField (Func<string> builder, string placeholder = null, Action<string> onEditingChanged = null, Action<string> onCommit = null )
		{
			TextBinding = builder;
			Placeholder = placeholder;
			OnEditingChanged = onEditingChanged;
			OnCommit = onCommit;
		}
		
		string text;
		public string Text {
			get => text;
			private set => this.SetValue (State, ref text, value, ViewPropertyChanged);
		}

		string placeholder;
		public string Placeholder {
			get => placeholder;
			set => this.SetValue (State, ref placeholder, value, ViewPropertyChanged);
		}

		public Func<string> TextBinding { get; private set; }
		protected override void WillUpdateView ()
		{
			base.WillUpdateView ();
			if (TextBinding != null) {
				State.StartProperty ();
				var text = TextBinding.Invoke ();
				var props = State.EndProperty ();
				var propCount = props.Length;
				if (propCount > 0) {
					State.BindingState.AddViewProperty (props, (s,o)=> Text = TextBinding.Invoke());
				}
				Text = text;
			}
		}
		
		public Action<TextField> Focused { get; private set; }
		public Action<TextField> Unfocused { get; private set; }
		public Action<string> OnEditingChanged { get; private set; }
		public Action<string> OnCommit { get; private set; }
	}
}

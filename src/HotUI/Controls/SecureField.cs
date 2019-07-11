using System;
namespace HotUI {
	public class SecureField : View 
	{
		public SecureField ()
		{

		}
			
		public SecureField (string placeholder, Action<string> onCommit = null) : base (true)
		{
			Placeholder = placeholder;
			OnCommit = onCommit;
		}
		
		public SecureField (string text = null, string placeholder = null, Action<string> onCommit = null) : base (true)
		{
			Text = text;
			Placeholder = placeholder;
			OnCommit = onCommit;
		}
		
		public SecureField (Func<string> builder, string placeholder = null, Action<string> onCommit = null )
		{
			TextBinding = builder;
			Placeholder = placeholder;
			OnCommit = onCommit;
		}

		string text;
		public string Text {
			get => text;
			private set => this.SetValue (State, ref text, value, ViewPropertyChanged);
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

		string placeholder;
		public string Placeholder {
			get => placeholder;
			set => this.SetValue (State, ref placeholder, value, ViewPropertyChanged);
		}
		
		public Action<string> OnCommit { get; set; }
	}
}

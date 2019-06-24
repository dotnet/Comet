using System;
namespace HotUI {
	public class TextField : View {
		public TextField ()
		{

		}
		public TextField (string text)
		{
			Text = text;
		}
		public TextField (Func<string> builder)
		{
			TextBinding = builder;
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

		public Action<string> Completed { get; set; }
		public Action<TextField> Focused { get; set; }
		public Action<TextField> Unfocused { get; set; }
		public Action<(string NewText, string OldText)> TextChanged { get; set; }
	}
}

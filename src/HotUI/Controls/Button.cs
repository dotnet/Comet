using System;

namespace HotUI 
{
	public class Button : View 
	{
		public Button ()
		{

		}
		
		public Button (string text, Action action = null) : base (true)
		{
			Text = text;
			OnClick = action;
		}
		
		public Button (Func<string> text, Action action = null)
		{
			TextBinding = text;
			OnClick = action;
		}
		
		private string _text;
		public string Text {
			get => _text;
			private set => this.SetValue (State, ref _text, value, ViewPropertyChanged);
		}

		public Action OnClick { get; private set; }

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
					State.BindingState.AddViewProperty (props, (s, o) => Text = TextBinding.Invoke ());
				}
				Text = text;
			}
		}
	}
}

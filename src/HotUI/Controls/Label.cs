using System;
using System.Diagnostics;
namespace HotUI {
	public class Label : View {
		public Label()
		{

		}
		public Label(string text)
		{
			Text = text;
		}
		public Label(Func<string> formatedText)
		{
			FormatedText = formatedText;
		}
		private string text;
		public string Text {
			get => text;
			set => this.SetValue (State, ref text, value, ViewPropertyChanged);
		}

		public Func<string> FormatedText { get; set; }

		protected override void WillUpdateView ()
		{
			base.WillUpdateView ();
			if (FormatedText != null) {
				State.StartProperty ();
				var text = FormatedText.Invoke ();
				var props = State.EndProperty ();
				var propCount = props.Length;
				if (propCount > 0) {
					State.BindingState.AddViewProperty (props, (s, o) => Text = FormatedText.Invoke ());
				}
				Text = text;
			}
		}

	}
}

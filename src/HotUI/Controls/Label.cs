using System;
using System.Diagnostics;
namespace HotUI {
	public class Label : View {

		private string text;
		public string Text {
			get => text;
			set => this.SetValue (State, ref text, value, ViewPropertyChanged);
		}

		public Func<string> TextBinding { get; set; }

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

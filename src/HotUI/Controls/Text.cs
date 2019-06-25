using System;
using System.Diagnostics;
namespace HotUI {
	public class Text : View {
		public Text()
		{

		}
		
		public Text(object value)
		{
			Value = value?.ToString();
		}
		
		public Text(string value)
		{
			Value = value;
		}
		
		public Text(Func<string> formatedText)
		{
			TextBinding = formatedText;
		}
		private string _value;
		public string Value {
			get => _value;
			private set => this.SetValue (State, ref _value, value, ViewPropertyChanged);
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
					State.BindingState.AddViewProperty (props, (s, o) => Value = TextBinding.Invoke ());
				}
				Value = text;
			}
		}

	}
}

using System;
using System.Diagnostics;
using FControlType = Xamarin.Forms.Label;
namespace HotForms {
	public class Label : View<FControlType> {
		public Label ()
		{
		}


		private string text;
		public string Text {
			get => text;
			set  => this.SetValue (State, ref text, value, updateText);
		}

		void updateText(object stringObject)
		{
			var value = (string)stringObject;
			text = value;
			if (IsControlCreated)
				FormsControl.Text = value;
			
		}

		void updateTextFromBinding(object stringObject)
		{
			if (IsControlCreated)
				FormsControl.Text = TextBinding.Invoke ();
		}
		public Func<string> TextBinding { get; set; }

		protected override object CreateFormsView ()
		{
			var control = (FControlType)base.CreateFormsView ();
			if (Text != null && TextBinding != null)
				throw new Exception ("Cannot use both Text and TextBinding");
			if (TextBinding != null) {
				State.StartProperty ();
				var text = TextBinding.Invoke ();
				var props = State.EndProperty ();
				var propCount = props.Length;
				if (propCount > 0) {
					State.BindingState.AddViewProperty (props, updateTextFromBinding);
				}
				control.Text = text;
				

				//We are going to figure out what was 
			} else {
				control.Text = Text;
			}

			return control;

		}


		internal void PropertyChanged (string property, object value)
		{

		}
	}
}

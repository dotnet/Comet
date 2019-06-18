using System;
using FControlType = Xamarin.Forms.Entry;
namespace HotForms {
	public class Entry : View<FControlType> {

		public Entry ()
		{

		}

		string text;
		public string Text {
			get => text;
			set => this.SetValue (State, ref text, value, updateText);
		}

		void updateText (object stringObject)
		{
			var value = (string)stringObject;
			text = value;
			if (IsControlCreated)
				FormsControl.Text = value;

		}
		public string Placeholder { get; set; }

		public Action<string> Completed { get; set; }
		public Action<Entry> Focused { get; set; }
		public Action<Entry> Unfocused { get; set; }
		public Action<(string NewText, string OldText)> TextChanged { get; set; }


		private void FormsControl_Completed (object sender, EventArgs e) => Completed?.Invoke (FormsControl.Text);

		private void FormsControl_Unfocused (object sender, Xamarin.Forms.FocusEventArgs e) => Unfocused?.Invoke (this);

		private void FormsControl_TextChanged (object sender, Xamarin.Forms.TextChangedEventArgs e) =>
			TextChanged?.Invoke ((e.NewTextValue, e.OldTextValue));

		private void FormsControl_Focused (object sender, Xamarin.Forms.FocusEventArgs e) => Focused?.Invoke (this);


		protected override void UnbindFormsView (object formsView)
		{

			var control = (FControlType)formsView;

			control.Focused -= FormsControl_Focused;
			control.TextChanged -= FormsControl_TextChanged;
			control.Unfocused -= FormsControl_Unfocused;
			control.Completed -= FormsControl_Completed;
			
		}

		protected override void UpdateFormsView (object formsView)
		{
			var control = (FControlType)formsView;

			control.Focused += FormsControl_Focused;
			control.TextChanged += FormsControl_TextChanged;
			control.Unfocused += FormsControl_Unfocused;
			control.Completed += FormsControl_Completed;

			control.Text = Text;
			control.Placeholder = Placeholder;
		}
	}
}

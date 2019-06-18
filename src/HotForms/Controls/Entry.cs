using System;
using FControlType = Xamarin.Forms.Entry;
namespace HotForms {
	public class Entry : View<FControlType> {
		private string text;

		public Entry ()
		{
		}
		State _state;
		public Entry (State state)
		{
			_state = state;
			state.StartBuildingView ();
		}

		public string Text {
			get => text;
			set {
				//TODO: move this to a magic extension
				//We can find magic bindings
				if (_state?.IsBuilding ?? false) {
					var props = _state.EndProperty ();
					var propCount = props.Length;
					//This is databound!
					if (propCount > 0) {
						bool isGlobal = propCount > 1;
						if (propCount == 1) {
							var prop = props [0];
							var stateValue = _state.GetValue (prop);
							//1 to 1 binding!
							if (value == stateValue) {
								_state.BindingState.AddViewProperty (prop, updateText);
							} else {
								isGlobal = true;
							}
						}

						if (isGlobal) {
							_state.BindingState.AddGlobalProperties (props);
						}
					}
				}
				text = value;
				updateText (value);
			}
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

		protected override object CreateFormsView ()
		{
			var control = (FControlType)base.CreateFormsView ();



			control.Focused += FormsControl_Focused;
			control.TextChanged += FormsControl_TextChanged;
			control.Unfocused += FormsControl_Unfocused;
			control.Completed += FormsControl_Completed;

			control.Text = Text;
			control.Placeholder = Placeholder;
			return control;
		}
	}
}

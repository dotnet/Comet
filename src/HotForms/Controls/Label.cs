using System;
using System.Diagnostics;
using FControlType = Xamarin.Forms.Label;
namespace HotForms {
	public class Label : View<FControlType> {
		public Label ()
		{
			_state = StateBuilder.CurrentState;
			_state.StartBuildingView ();
		}

		State _state;

		private string text;
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
								Debug.WriteLine ($"Warning: {nameof (Text)} is using formated Text. For performance reasons, please switch to TextBinding");
								isGlobal = true;
							}
						} else {
							Debug.WriteLine ($"Warning: {nameof (Text)} is using Multiple state Variables. For performance reasons, please switch to TextBinding");
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
			State state = _state;
			if (Text != null && TextBinding != null)
				throw new Exception ("Cannot use both Text and TextBinding");
			if (TextBinding != null) {
				state.StartProperty ();
				var text = TextBinding.Invoke ();
				var props = state.EndProperty ();
				var propCount = props.Length;
				if (propCount > 0) {
					_state.BindingState.AddViewProperty (props, updateTextFromBinding);
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

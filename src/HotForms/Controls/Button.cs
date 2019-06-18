using System;
using Xamarin.Forms;
using FControlType = Xamarin.Forms.Button;
namespace HotForms {
	public class Button : View<FControlType> {

		private string text;
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

		public Action OnClick { get; set; }

		protected override void UnbindFormsView (object formsView)
		{

			var button = formsView as FControlType;
			if (button == null)
				return;
			button.Command = null;

		}

		protected override void UpdateFormsView (object formsView)
		{
			var button = (FControlType)formsView;
			button.Command = new Command ((s) =>  OnClick?.Invoke ());
			button.Text = Text;
		}
	}
}

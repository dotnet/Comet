using System;
using FControlType = Xamarin.Forms.Label;
namespace HotForms {
	public class Label : BaseControl<FControlType> {
		public Label()
		{

		}
		public Label(string text)
		{
			Text = text;
		}
		public string Text {
			get => FormsControl.Text;
			set => FormsControl.Text = value;
		}
	}
}

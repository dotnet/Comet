using System;
using FControlType = Xamarin.Forms.Label;
namespace HotForms {
	public class Label : BaseControl<FControlType> {
		public string Text {
			get => FormsControl.Text;
			set => FormsControl.Text = value;
		}
	}
}

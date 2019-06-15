using System;
using FControl = Xamarin.Forms.Label;
namespace HotForms {
	public class Label : BaseControl<FControl> {
		public string Text {
			get => FormsControl.Text;
			set => FormsControl.Text = value;
		}
	}
}

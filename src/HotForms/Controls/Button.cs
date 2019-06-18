using System;
using Xamarin.Forms;
using FControlType = Xamarin.Forms.Button;
namespace HotForms {
	public class Button : View<FControlType> {
		public string Text {
			get => FormsControl.Text;
			set => FormsControl.Text = value;
		}
		Action onClick;
		public Action OnClick {
			get => onClick;
			set {
				onClick = value;
				FormsControl.Command = onClick == null ? null : new Command (onClick);
			}
		}
	}
}

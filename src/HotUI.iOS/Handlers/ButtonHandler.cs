using System;
using UIKit;

namespace HotUI.iOS {
	public class ButtonHandler : UIButton, IViewHandler, IUIView {
		public ButtonHandler ()
		{
			this.TouchUpInside += ButtonHandler_TouchUpInside;
		}

		private void ButtonHandler_TouchUpInside (object sender, EventArgs e) => button?.OnClick ();

		public UIView View => this;

		Button button;
		public void Remove (View view)
		{

		}

		public void SetView (View view)
		{
			button = view as Button;
			this.UpdateProperties (button);
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}
	}

	public static partial class ControlExtensions {

		public static void UpdateProperties (this UIButton view, Button hView)
		{
			view.SetTitle (hView?.Text, UIControlState.Normal);
			view.UpdateBaseProperties (hView);
		}

		public static bool UpdateProperty (this UIButton view, string property, object value)
		{
			switch (property) {
			case nameof (Button.Text):
				view.SetTitle ((string)value, UIControlState.Normal);
				return true;
			}
			return view.UpdateBaseProperty (property, value);
		}
	}
}

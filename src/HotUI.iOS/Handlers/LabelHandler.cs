using System;
using UIKit;

namespace HotUI.iOS {
	public class LabelHandler : UILabel, IViewHandler, IUIView {

		public UIView View => this;

		public void Remove (View view)
		{

		}

		public void SetView (View view)
		{
			var label = view as Label;
			this.UpdateProperties (label);

		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}
	}

	public static partial class ControlExtensions {

		public static void UpdateProperties (this UILabel view, Label hView)
		{
			view.Text = hView?.Text;
			view.UpdateBaseProperties (hView);
		}

		public static bool UpdateProperty (this UILabel view, string property, object value)
		{
			switch (property) {
			case nameof (Label.Text):
				view.Text = (string)value;
				return true;
			}
			return view.UpdateBaseProperty (property, value);
		}
	}
}

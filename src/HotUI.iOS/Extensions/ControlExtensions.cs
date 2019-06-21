using System;
using UIKit;

namespace HotUI.iOS {
	public static partial class ControlExtensions {
		public static void UpdateProperties (this UIView view, View hView)
		{

		}
		public static void UpdateBaseProperties (this UIView view, View hView)
		{
			view.UpdateProperties (hView);
		}

		public static bool UpdateProperty (this UIView view, string property, object value)
		{
			return false;
		}
		public static bool UpdateBaseProperty (this UIView view, string property, object value) => view.UpdateProperty (property, value);


	}
}

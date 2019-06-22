using AppKit;

namespace HotUI.Mac.Extensions {
	public static partial class ControlExtensions {
		public static void UpdateProperties (this NSView view, View hView)
		{

		}
		public static void UpdateBaseProperties (this NSView view, View hView)
		{
			view.UpdateProperties (hView);
		}

		public static bool UpdateProperty (this NSView view, string property, object value)
		{
			return false;
		}
		public static bool UpdateBaseProperty (this NSView view, string property, object value) => view.UpdateProperty (property, value);


	}
}

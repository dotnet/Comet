using System;
namespace HotUI {
	public class ModalView : ContentView {

		public static void Dismiss () => PerformDismiss?.Invoke ();

		public static Action PerformDismiss;
	}
}

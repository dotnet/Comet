using System;
namespace Comet
{
	public class ModalView : ContentView
	{

		public static void Dismiss() => PerformDismiss?.Invoke();
		public static Action PerformDismiss;

		public static void Present(View view) => PerformPresent?.Invoke(view);
		public static Action<View> PerformPresent;
	}
}

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo ("HotUI.Tests")]

namespace HotUI{
	public static class HotUI {
		public static Action<Action> PerformInvokeOnMainThread;
		public static void InvokeOnMainThread (Action action) => PerformInvokeOnMainThread (action);
	}
}
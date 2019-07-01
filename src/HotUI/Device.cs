using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo ("HotUI.Tests")]

namespace HotUI{
	public static class Device {
		public static Action<Action> PerformInvokeOnMainThread;
		public static void InvokeOnMainThread (Action action) => PerformInvokeOnMainThread (action);
	}
}
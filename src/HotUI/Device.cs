using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo ("HotUI.Tests")]

namespace HotUI{
	public static class Device {

		static Device()
		{
			mainThread = Thread.CurrentThread;
		}
		public static Action<Action> PerformInvokeOnMainThread;
		internal static Thread mainThread;
		public static void InvokeOnMainThread (Action action)
		{
			if (mainThread == Thread.CurrentThread)
				action ();
			else
				PerformInvokeOnMainThread (action);
		}
	}
}
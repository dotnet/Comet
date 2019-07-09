using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using HotUI.Services;

[assembly: InternalsVisibleTo ("HotUI.Tests")]
[assembly: InternalsVisibleTo ("HotUI.Reload.Tests")]
[assembly: InternalsVisibleTo ("HotUI.Reload.NetCore.Tests")]

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

		public static IFontService FontService = new FallbackFontService();
	}
}
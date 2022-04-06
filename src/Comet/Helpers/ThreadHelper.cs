using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;

namespace Comet
{
	public class ThreadHelper
	{
		public static void SetFireOnMainThread(Action<Action> action) => FireOnMainThread = action;
		static Action<Action> FireOnMainThread = MainThread.BeginInvokeOnMainThread;
		public static void RunOnMainThread(Action action) => FireOnMainThread.Invoke(action);
	}
}
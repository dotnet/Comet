
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Comet
{
	public class ThreadHelper
	{
		public static bool IsMainThread => MainThread == Thread.CurrentThread;
		static Thread MainThread;
		public static void Setup(Action<Action> action)
		{
			FireOnMainThread = action;
			MainThread = Thread.CurrentThread;
		}
		static Action<Action> FireOnMainThread;
		public static void RunOnMainThread(Action action)
		{
			if (!IsMainThread)
			{
				FireOnMainThread(action);
			}
			else
				action();
		}

	}
}

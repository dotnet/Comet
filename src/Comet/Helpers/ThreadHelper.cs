using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Comet
{
	public class ThreadHelper
	{
		public static bool IsMainThread => (joinableTaskContext?.MainThread ?? Thread.CurrentThread) == Thread.CurrentThread;
		static JoinableTaskContext joinableTaskContext;
		public static JoinableTaskContext JoinableTaskContext
		{
			get => joinableTaskContext ?? (joinableTaskContext = new JoinableTaskContext());
			set => joinableTaskContext = value;
		}


		//
		// Summary:
		//     Gets an awaitable whose continuations execute on the synchronization context
		//     that this instance was initialized with, in such a way as to mitigate both deadlocks
		//     and reentrancy.
		//
		// Parameters:
		//   alwaysYield:
		//     A value indicating whether the caller should yield even if already executing
		//     on the main thread.
		//
		//   cancellationToken:
		//     A token whose cancellation will immediately schedule the continuation on a threadpool
		//     thread.
		//
		// Returns:
		//     An awaitable.
		//
		// Exceptions:
		//   T:System.OperationCanceledException:
		//     Thrown back at the awaiting caller from a background thread when cancellationToken
		//     is canceled before any required transition to the main thread is complete. No
		//     exception is thrown if alwaysYield is false and the caller was already on the
		//     main thread before calling this method, or if the main thread transition completes
		//     before the thread pool responds to cancellation.
		//
		// Remarks:
		//     private async Task SomeOperationAsync() { // This first part can be on the caller's
		//     thread, whatever that is. DoSomething(); // Now switch to the Main thread to
		//     talk to some STA object. // Supposing it is also important to *not* do this step
		//     on our caller's callstack, // be sure we yield even if we're on the UI thread.
		//     await this.JoinableTaskFactory.SwitchToMainThreadAsync(alwaysYield: true); STAService.DoSomething();
		//     }
		public static JoinableTaskFactory.MainThreadAwaitable SwitchToMainThreadAsync(bool alwaysYield, CancellationToken cancellationToken = default) => JoinableTaskContext.Factory.SwitchToMainThreadAsync(alwaysYield, cancellationToken);

		//
		// Summary:
		//     Gets an awaitable whose continuations execute on the synchronization context
		//     that this instance was initialized with, in such a way as to mitigate both deadlocks
		//     and reentrancy.
		//
		// Parameters:
		//   cancellationToken:
		//     A token whose cancellation will immediately schedule the continuation on a threadpool
		//     thread (if the transition to the main thread is not already complete). The token
		//     is ignored if the caller was already on the main thread.
		//
		// Returns:
		//     An awaitable.
		//
		// Exceptions:
		//   T:System.OperationCanceledException:
		//     Thrown back at the awaiting caller from a background thread when cancellationToken
		//     is canceled before any required transition to the main thread is complete. No
		//     exception is thrown if the caller was already on the main thread before calling
		//     this method, or if the main thread transition completes before the thread pool
		//     responds to cancellation.
		//
		// Remarks:
		//     private async Task SomeOperationAsync() { // on the caller's thread. await DoAsync();
		//     // Now switch to a threadpool thread explicitly. await TaskScheduler.Default;
		//     // Now switch to the Main thread to talk to some STA object. await this.JobContext.SwitchToMainThreadAsync();
		//     STAService.DoSomething(); }
		public static JoinableTaskFactory.MainThreadAwaitable SwitchToMainThreadAsync(CancellationToken cancellationToken = default) => JoinableTaskContext.Factory.SwitchToMainThreadAsync(cancellationToken);
		public static void SetFireOnMainThread(Action<Action> action) => FireOnMainThread = action;
		static Action<Action> FireOnMainThread;
		public static async void RunOnMainThread(Action action)
		{
			if (!IsMainThread)
			{
				if (FireOnMainThread != null)
				{
					FireOnMainThread(action);
					return;
				}
				await SwitchToMainThreadAsync(false);
			}
			action();
		}

		//
		// Summary:
		//     Runs the specified asynchronous method to completion while synchronously blocking
		//     the calling thread.
		//
		// Parameters:
		//   asyncMethod:
		//     The asynchronous method to execute.
		//
		// Type parameters:
		//   T:
		//     The type of value returned by the asynchronous operation.
		//
		// Returns:
		//     The result of the Task returned by asyncMethod.
		//
		// Remarks:
		//     Any exception thrown by the delegate is rethrown in its original type to the
		//     caller of this method.
		//     When the delegate resumes from a yielding await, the default behavior is to resume
		//     in its original context as an ordinary async method execution would. For example,
		//     if the caller was on the main thread, execution resumes after an await on the
		//     main thread; but if it started on a threadpool thread it resumes on a threadpool
		//     thread.
		//     See the Microsoft.VisualStudio.Threading.JoinableTaskFactory.Run(System.Func{System.Threading.Tasks.Task})
		//     overload documentation for an example.
		public static T Run<T>(Func<Task<T>> asyncMethod) => JoinableTaskContext.Factory.Run(asyncMethod);
		//
		// Summary:
		//     Runs the specified asynchronous method to completion while synchronously blocking
		//     the calling thread.
		//
		// Parameters:
		//   asyncMethod:
		//     The asynchronous method to execute.
		//
		// Remarks:
		//     Any exception thrown by the delegate is rethrown in its original type to the
		//     caller of this method.
		//     When the delegate resumes from a yielding await, the default behavior is to resume
		//     in its original context as an ordinary async method execution would. For example,
		//     if the caller was on the main thread, execution resumes after an await on the
		//     main thread; but if it started on a threadpool thread it resumes on a threadpool
		//     thread.
		//     // On threadpool or Main thread, this method will block // the calling thread
		//     until all async operations in the // delegate complete. joinableTaskFactory.Run(async
		//     delegate { // still on the threadpool or Main thread as before. await OperationAsync();
		//     // still on the threadpool or Main thread as before. await Task.Run(async delegate
		//     { // Now we're on a threadpool thread. await Task.Yield(); // still on a threadpool
		//     thread. }); // Now back on the Main thread (or threadpool thread if that's where
		//     we started). });
		public static void Run(Func<Task> asyncMethod) => JoinableTaskContext.Factory.Run(asyncMethod);

		//
		// Summary:
		//     Invokes an async delegate on the caller's thread, and yields back to the caller
		//     when the async method yields. The async delegate is invoked in such a way as
		//     to mitigate deadlocks in the event that the async method requires the main thread
		//     while the main thread is blocked waiting for the async method's completion.
		//
		// Parameters:
		//   asyncMethod:
		//     The method that, when executed, will begin the async operation.
		//
		// Returns:
		//     An object that tracks the completion of the async operation, and allows for later
		//     synchronous blocking of the main thread for completion if necessary.
		//
		// Remarks:
		//     Exceptions thrown by the delegate are captured by the returned Microsoft.VisualStudio.Threading.JoinableTask.
		//     When the delegate resumes from a yielding await, the default behavior is to resume
		//     in its original context as an ordinary async method execution would. For example,
		//     if the caller was on the main thread, execution resumes after an await on the
		//     main thread; but if it started on a threadpool thread it resumes on a threadpool
		//     thread.
		public static JoinableTask RunAsync(Func<Task> asyncMethod) => JoinableTaskContext.Factory.RunAsync(asyncMethod);

		//
		// Summary:
		//     Invokes an async delegate on the caller's thread, and yields back to the caller
		//     when the async method yields. The async delegate is invoked in such a way as
		//     to mitigate deadlocks in the event that the async method requires the main thread
		//     while the main thread is blocked waiting for the async method's completion.
		//
		// Parameters:
		//   asyncMethod:
		//     The method that, when executed, will begin the async operation.
		//
		// Type parameters:
		//   T:
		//     The type of value returned by the asynchronous operation.
		//
		// Returns:
		//     An object that tracks the completion of the async operation, and allows for later
		//     synchronous blocking of the main thread for completion if necessary.
		//
		// Remarks:
		//     Exceptions thrown by the delegate are captured by the returned Microsoft.VisualStudio.Threading.JoinableTask.
		//     When the delegate resumes from a yielding await, the default behavior is to resume
		//     in its original context as an ordinary async method execution would. For example,
		//     if the caller was on the main thread, execution resumes after an await on the
		//     main thread; but if it started on a threadpool thread it resumes on a threadpool
		//     thread.

		public static JoinableTask<T> RunAsync<T>(Func<Task<T>> asyncMethod) => JoinableTaskContext.Factory.RunAsync(asyncMethod);


	}
}

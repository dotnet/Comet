using System;
using System.Threading.Tasks;

namespace Comet
{
    public class ResultView<T> : ContentView
    {
        TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

        public Task<T> GetResult() => tcs.Task;

        public void Cancel() => tcs.TrySetCanceled();

        public void SetResult(T value) => tcs.TrySetResult(value);

        public void SetException(Exception ex) => tcs.TrySetException(ex);

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                tcs.TrySetCanceled();
            }
            base.Dispose(disposing);
        }


    }
}

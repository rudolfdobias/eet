using System;
using System.Threading.Tasks;

namespace Mews.Eet
{
    public static class AsyncHelpers
    {
        public static Task<TOut> SafeContinuationAction<TIn, TOut>(Task<TIn> task, Func<TIn, TOut> continuationAction)
        {
            var completionSource = new TaskCompletionSource<TOut>();
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    completionSource.TrySetException(t.Exception.InnerExceptions);
                }
                else if (t.IsCanceled)
                {
                    completionSource.TrySetCanceled();
                }
                else
                {
                    var result = continuationAction(t.Result);
                    completionSource.TrySetResult(result);
                }
            });
            return completionSource.Task;
        }
    }
}

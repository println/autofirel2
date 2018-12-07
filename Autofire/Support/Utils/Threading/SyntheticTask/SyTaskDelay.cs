using System.Threading;
using System.Threading.Tasks;

namespace Autofire.Support.Utils.Threading.SyntheticTask
{
    internal class SyTaskDelay
    {
        public static Task Delay(uint milliseconds, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<object>();
            token.Register(() => tcs.TrySetCanceled());
            new Timer(_ => tcs.TrySetResult(null)).Change(milliseconds, -1);
            return tcs.Task;
        }
    }
}

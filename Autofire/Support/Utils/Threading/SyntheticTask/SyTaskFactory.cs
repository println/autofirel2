using System;
using System.Threading;
using System.Threading.Tasks;

namespace Autofire.Support.Utils.Threading.SyntheticTask
{
    public class TaskFactory
    {
        internal TaskFactory() { }

        public SyTask NewLoopTask(Action action, uint milliseconds)
        {
            return CreateTask((token) => CreateLoopTask(action, milliseconds, token));
        }

        public SyTask NewCustomTask(Action<Action<uint>> action)
        {
            return CreateTask((token) => CreateCustomTask(action, token));
        }

        private SyTask CreateTask(Func<CancellationToken, Task> createTask)
        {
            var cts = new CancellationTokenSource();
            return new SyTask(createTask(cts.Token), cts);
        }

        private Task CreateLoopTask(Action action, uint milliseconds, CancellationToken token)
        {
            return new Task(() =>
            {
                try
                {
                    while (true)
                    {
                        token.ThrowIfCancellationRequested();
                        SyTaskDelay.Delay(milliseconds, token).Wait();
                        action();
                    }
                }
                catch { }

            }, token);
        }

        private Task CreateCustomTask(Action<Action<uint>> action, CancellationToken token)
        {
            return new Task(() =>
            {
                try
                {
                    action((milliseconds) =>
                    {
                        token.ThrowIfCancellationRequested();
                        SyTaskDelay.Delay(milliseconds, token).Wait();
                    });
                }
                catch { }

            }, token);
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Autofire.Support.Utils.Threading.SyntheticTask
{
    public class SyTask : IDisposable
    {
        public static TaskFactory Factory {
            get
            {
                return new TaskFactory();
            }
        }

        public bool IsRunning { get; private set; }

        public bool IsFinished
        {
            get
            {
                return cts == null;
            }
        }

        private CancellationTokenSource cts;
        private Task task;

        public SyTask(Task task, CancellationTokenSource cts)
        {
            this.task = task;
            this.cts = cts;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                task.Start();
                IsRunning = true;
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                cts.Cancel();
                cts.Dispose();
                task.Wait();
                cts = null;
                IsRunning = false;
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}

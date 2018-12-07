using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofire.Support.Utils.Threading.SyntheticTask;

namespace Autofire.Support.Daemons.Game.Factory
{
    internal class TaskedGameDaemon : IGameDaemon
    {
        public event GameEventHandler GameClosedEvent = delegate { };
        public event GameEventHandler GameStartedEvent = delegate { };

        private string windowTitle;
        private SyTask syTask;

        private IDictionary<int, string> processTable;

        internal TaskedGameDaemon(string windowTitle, uint intervalInSeconds)
        {
            this.windowTitle = windowTitle;
            this.processTable = GetProcesses();
            this.syTask = SyTask.Factory.NewLoopTask(() => FindProcesses(), intervalInSeconds * 1000);
        }

        internal TaskedGameDaemon(string windowTitle) : this(windowTitle, 2) { }

        public void Start()
        {
            if (!this.syTask.IsRunning)
            {
                this.syTask.Start();
            }
        }

        public void Stop()
        {
            if (this.syTask.IsRunning)
            {
                this.syTask.Stop();
            }
        }

        public IDictionary<int, string> Scan()
        {
            return this.processTable.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        public void Dispose()
        {
            Stop();
        }

        private void FindProcesses()
        {
            var currentProcessTable = GetProcesses();

            currentProcessTable.Except(this.processTable)
               .ToList()
               .ForEach(p => GameStartedEvent(p.Value, p.Key, GameStatus.Started));

            this.processTable.Except(currentProcessTable)
                .ToList()
                .ForEach(p => GameClosedEvent(p.Value, p.Key, GameStatus.Closed));

            this.processTable = currentProcessTable;
        }

        private IDictionary<int, string> GetProcesses()
        {
            return System.Diagnostics.Process.GetProcesses()
                   .Where(p => p.MainWindowTitle.Equals(this.windowTitle, StringComparison.OrdinalIgnoreCase))
                   .ToDictionary(p => p.Id, p => Path.GetFileName(p.MainModule.FileName));

        }
    }
}

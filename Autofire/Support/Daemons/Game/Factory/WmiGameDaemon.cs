using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;

namespace Autofire.Support.Daemons.Game.Factory

{
    internal class WmiGameDaemon : IGameDaemon
    {

        private const string SCOPE = @"\\.\root\CIMV2";
        private const string QUERY = "SELECT * FROM __InstanceOperationEvent " +
            "WITHIN  {0} WHERE TargetInstance ISA 'Win32_Process'  " +
            "AND TargetInstance.Name = '{1}'";

        private string processName;
        private uint interval = 2;
        private ManagementEventWatcher watcher;

        public event GameEventHandler GameClosedEvent;
        public event GameEventHandler GameStartedEvent;

        internal WmiGameDaemon(string name)
        {
            processName = name;
        }

        internal WmiGameDaemon(string name, uint intervalInSeconds) : this(name)
        {
            interval = intervalInSeconds;
        }

        public void Start()
        {
            if (watcher == null)
            {
                watcher = new ManagementEventWatcher(SCOPE, String.Format(QUERY, interval, processName));
                watcher.EventArrived += WatchEventArrived;
                watcher.Start();
            }
        }

        public IDictionary<int, string> Scan()
        {
            string ext = Path.GetExtension(processName);
            string name = Path.GetFileNameWithoutExtension(processName);
            return System.Diagnostics.Process.GetProcessesByName(name).ToDictionary(p => p.Id, p => p.ProcessName + ext);
        }

        public void Stop()
        {
            if (watcher != null)
            {
                watcher.Stop();
                watcher.EventArrived -= WatchEventArrived;
                watcher = null;
            }
        }

        public void Dispose()
        {
            Stop();
        }

        private void WatchEventArrived(object sender, EventArrivedEventArgs e)
        {
            string eventName = e.NewEvent.ClassPath.ClassName;

            switch (eventName)
            {
                case "__InstanceCreationEvent":
                    TriggerEvent(e, GameStartedEvent, GameStatus.Started);
                    break;
                case "__InstanceDeletionEvent":
                    TriggerEvent(e, GameClosedEvent, GameStatus.Closed);
                    break;
            }
        }

        private void TriggerEvent(EventArrivedEventArgs e, GameEventHandler trigger, GameStatus state)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;

            var name = targetInstance.Properties["Name"].Value.ToString();
            var id = Int32.Parse(targetInstance.Properties["ProcessID"].Value.ToString());

            trigger(name, id, state);
        }
    }
}

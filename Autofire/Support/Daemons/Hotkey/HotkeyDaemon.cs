using System;
using System.Windows.Forms;
using Autofire.Support.Daemons.Hotkey.KeyboardHook;

namespace Autofire.Support.Daemons.Hotkey
{
    public class HotkeyDaemon : IMatchmaker, IHotkeyDaemon
    {
        public event KeyEventHandler HotkeyCaptured = delegate { };

        public HotkeyResolver Resolver { get; } = new HotkeyResolver();

        private LowLevelHook hook;

        public HotkeyDaemon()
        {
            hook = new LowLevelHook(this);
        }

        public void Start()
        {
            if (!hook.IsRunning)
            {
                hook.Start();
            }
        }

        public void Stop()
        {
            if (hook.IsRunning)
            {
                hook.Stop();
            }
        }

        public void Dispose()
        {
            Stop();
        }

        bool IMatchmaker.HasAMatching(int key)
        {
            if (Resolver.IsValid(key))
            {
                Notify(key);
                return true;
            }
            return false;
        }

        private void Notify(int key)
        {
            var root = new KeyEventHandler(HotkeyCaptured);
            root.BeginInvoke(this, new KeyEventArgs((Keys)key), EndAsyncEvent, null);
        }

        private void EndAsyncEvent(IAsyncResult iar)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)iar;
            var invokedMethod = (KeyEventHandler)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch { }
        }
    }
}

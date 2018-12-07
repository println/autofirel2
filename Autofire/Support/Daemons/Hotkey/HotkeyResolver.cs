using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Autofire.Support.Daemons.Hotkey
{
    public class HotkeyResolver
    {
        public int ProcessId { get; set; }

        private ICollection<int> keys;

        internal HotkeyResolver()
        {
            keys = new HashSet<int>();
        }

        public void Add(int hotkey)
        {
            keys.Add(hotkey);
        }

        public void Remove(int hotkey)
        {
            keys.Remove(hotkey);
        }

        internal bool IsValid(int key)
        {
            return (HasFocus() && keys.Contains(key));
        }

        private bool HasFocus()
        {
            if (ProcessId == 0)
            {
                return true;
            }
            return GetFocusedProcessId() == ProcessId;
        }

        public uint GetFocusedProcessId()
        {
            uint processID = 0;
            GetWindowThreadProcessId(GetForegroundWindow(), out processID);
            return processID;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    }
}

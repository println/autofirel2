using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Autofire.Support.Utils.Threading.Window
{
    public class WindowHandler
    {
        public void Focus(int processId)
        {
            Focus(GetHandler(processId));
        }

        public void Focus(IntPtr handle)
        {
            const int SW_RESTORE = 9;
            ShowWindow(handle, SW_RESTORE);
            SetForegroundWindow(handle);
        }

        public bool HasFocus(int processId)
        {
            return HasFocus(GetHandler(processId));
        }

        public bool HasFocus(IntPtr handle)
        {
            return handle == GetForegroundWindow();
        }

        private IntPtr GetHandler(int processId)
        {
            return Process.GetProcessById(processId).MainWindowHandle;
        }

        [DllImport("user32.dll")] //Restores window
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")] //Brings window to front
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
    }
}
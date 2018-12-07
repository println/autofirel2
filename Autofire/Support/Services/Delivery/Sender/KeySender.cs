using System;
using System.Runtime.InteropServices;

namespace  Autofire.UI.Components.Delivery.Sender
{
    internal class KeySender
    {
        const Int32 WM_KEYDOWN = 0x100;
        const Int32 WM_SYSKEYDOWN = 0x0104;
        const Int32 WM_KEYUP = 0x101;
        const Int32 WM_SYSTEMKEYUP = 0x0105;

        public void SendKey(IntPtr handle, int key)
        {
            PostMessage(handle, WM_SYSKEYDOWN, key, 0x00440001);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

    }
}

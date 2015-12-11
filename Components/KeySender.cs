using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AutoFire
{
    class KeySender
    {
        private static KeySender instance;

        internal static KeySender Instance
        {
            get
            {
                if (KeySender.instance == null)
                    KeySender.instance = new KeySender();
                return KeySender.instance;
            }
        }

        private KeySender(){}

        public void SendKey(object key)
        {
            const Int32 WM_KEYDOWN = 0x100;
            const Int32 WM_SYSKEYDOWN = 0x0104;
            //const Int32 WM_KEYUP = 0x101;

            IntPtr selectedProcess = WindowSeeker.GetProcess().MainWindowHandle;
            PostMessage(selectedProcess, WM_SYSKEYDOWN, (Keys)key, 1);
        }
        
        ~KeySender()
        {
            instance = null;
        }

        #region ImportDLLs

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, System.Windows.Forms.Keys wParam, int lParam);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace AutoFire
{
    class Observer
    {
        #region Variables
        //Modifier key constants
        private const int VK_SHIFT = 0x10;
        private const int VK_CONTROL = 0x11;
        private const int VK_MENU = 0x12;
        private const int VK_CAPITAL = 0x14;

        //Keyboard API constants
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        // Structure returned by the hook whenever a key is pressed
        internal struct KEYBOARD
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        //Variables used in the call to SetWindowsHookEx
        public delegate IntPtr KeyboardDelegate(int Code, IntPtr wParam, ref KEYBOARD lParam);
        private KeyboardDelegate callback;
        private IntPtr hookID;

        //Variables used in class
        private static Observer instance;
        private bool global;
        private List<Macro> macros = new List<Macro>();
        private List<Keys> keys = new List<Keys>();

        internal static Observer Instance
        {
            get
            {
                if (Observer.instance == null)
                    Observer.instance = new Observer();
                return Observer.instance;
            }
        }

        #endregion

        private Observer() { }

        public void Add(Macro macro)
        {
            macros.Add(macro);
            keys.Add((Keys)macro.GetKey());

            if (keys.Count == 1)
                hook();
        }

        public bool Remove(Macro macro)
        {
            keys.Remove((Keys)macro.GetKey());

            if (keys.Count < 1)
                unhook();

            return macros.Remove(macro);
        }

        public void Global(bool g)
        {
            global = g;
        }

        private bool hook()
        {
            callback = new KeyboardDelegate(HookCallback);
            IntPtr hInstance = LoadLibrary("User32");
            hookID = SetWindowsHookEx(WH_KEYBOARD_LL, callback, hInstance, 0);

            bool result = hookID != IntPtr.Zero ? true : false;

            return result;
        }

        private void unhook()
        {
            if (hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(hookID);
                hookID = IntPtr.Zero;
            }
        }

        private IntPtr HookCallback(int Code, IntPtr wParam, ref KEYBOARD lParam)
        {
            IntPtr activeProcess = GetForegroundWindow();
            IntPtr selectedProcess = WindowSeeker.GetProcess().MainWindowHandle;

            if (Code >= 0 &&
               (global || activeProcess == selectedProcess) &&
               (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN) &&
               !(lParam.vkCode >= 160 && lParam.vkCode <= 164))
            {
                Keys key = ((Keys)lParam.vkCode) | CheckModifiers();

                if (keys.Contains(key))
                {
                    for (int i = 0; i < keys.Count; i++)
                    {
                        if (key == keys[i])
                            macros[i].Notify();
                    }
                    return (IntPtr)(-1);
                }
            }
            return CallNextHookEx(hookID, Code, wParam, ref lParam);
        }

        private Keys CheckModifiers()
        {
            Keys key = Keys.None;

            //if ((GetKeyState(VK_CAPITAL) & 0x0001) != 0)//CAPSLOCK is ON
            //key = Keys.Capital | key;

            if ((GetKeyState(VK_SHIFT) & 0x8000) != 0)//SHIFT is pressed
                key = Keys.Shift | key;

            if ((GetKeyState(VK_CONTROL) & 0x8000) != 0) //CONTROL is pressed
                key = Keys.Control | key;

            if ((GetKeyState(VK_MENU) & 0x8000) != 0) //ALT is pressed
                key = Keys.Alt | key;

            return key;
        }

        ~Observer()
        {
            unhook();
            instance = null;
        }

        #region ImportDLLs

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int idHook, KeyboardDelegate callback, IntPtr hInstance, uint theardID);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr CallNextHookEx(IntPtr hhk, int code, IntPtr wParam, ref KEYBOARD lParam);
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        #endregion

    }


}

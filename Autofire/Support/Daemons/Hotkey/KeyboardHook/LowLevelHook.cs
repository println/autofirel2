using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Autofire.Support.Daemons.Hotkey.KeyboardHook
{
    internal class LowLevelHook : IDisposable
    {
        private const int WhKeyboardLl = 13;
        private const int WmKeyDown = 0x0100;
        private const int WmKeyUp = 0x0101;
        private const int WmSysKeyDown = 0x0104;
        private const int WmSysKeyUp = 0x0105;

        public bool IsRunning
        {
            get
            {
                return hookId != IntPtr.Zero;
            }
        }

        private IntPtr hookId;
        private IMatchmaker resolver;

        public LowLevelHook(IMatchmaker resolver)
        {
            this.resolver = resolver;
        }

        public void Start()
        {
            hookId = SetHook(HookCallback);
        }

        public void Stop()
        {
            UnhookWindowsHookEx(hookId);
        }

        public void Dispose()
        {
            Stop();
        }

        private IntPtr SetHook(LowLevelKeyboardCallback callback)
        {
            using (var curProcess = Process.GetCurrentProcess())
            {
                using (var curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WhKeyboardLl, callback, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        private delegate IntPtr LowLevelKeyboardCallback(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (HasValidState(nCode, wParam) && IsNotModifier(lParam))
            {
                if (ShouldStopPropagation(lParam))
                {
                    return (IntPtr)(-1);
                }
            }
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private bool ShouldStopPropagation(IntPtr lParam)
        {
            return resolver.HasAMatching(Marshal.ReadInt32(lParam) | Modifers.Scan());
        }

        private bool IsNotModifier(IntPtr lParam)
        {
            return (Marshal.ReadInt32(lParam) < 160 || Marshal.ReadInt32(lParam) > 164);
        }

        private bool HasValidState(int nCode, IntPtr wParam)
        {
            return nCode >= 0 && (IsKeyDown(wParam));
        }

        private bool IsKeyDown(IntPtr wParam)
        {
            return (wParam == (IntPtr)WmKeyDown || wParam == (IntPtr)WmSysKeyDown);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardCallback lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

    }

}

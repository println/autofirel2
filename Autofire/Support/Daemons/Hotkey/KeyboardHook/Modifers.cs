using System.Runtime.InteropServices;

namespace Autofire.Support.Daemons.Hotkey.KeyboardHook
{
    internal static class Modifers
    {
        //Modifiers constants
        public const int VkShift = 0x10;
        public const int VkControl = 0x11;
        public const int VkMenu = 0x12;
        public const int VkCapital = 0x14;

        //Modifiers keys
        public const int None = 0;
        public const int Capital = 20;
        public const int Shift = 65536;
        public const int Control = 131072;
        public const int Alt = 262144;

        public static int Scan()
        {
            return  GetShift() | GetControl() | GetAlt();
        }

        private static int GetControl()
        {
            return ((GetKeyState(VkControl) & 0x8000) != 0) ? Control : None;
        }

        private static int GetShift()
        {
            return ((GetKeyState(VkShift) & 0x8000) != 0) ? Shift : None;
        }

        private static int GetAlt()
        {
            return ((GetKeyState(VkMenu) & 0x8000) != 0) ? Alt : None;
        }

        private static int GetCapsLock()
        {
            return ((GetKeyState(VkCapital) & 0x0001) != 0) ? Capital : None;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);
    }
}

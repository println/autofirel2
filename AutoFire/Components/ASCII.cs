using System;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;

namespace AutoFire
{
    class ASCII
    {
        public static string Converter(KeyEventArgs key)
        {
            string result = "";

            if (!char.IsControl((char)key.KeyCode))
            {
                if (key.Control)
                    result += "CTRL";
                if (key.Shift)
                    result += result.Equals("") ? "SHIFT" : " + SHIFT";
                if (key.Alt)
                    result += result.Equals("") ? "ALT" : " + ALT";

                result += result.Equals("") ? ToAscii(key.KeyCode) : " + " + ToAscii(key.KeyCode);
            }

            return result;
        }

        private static string ToAscii(Keys key)
        {
            if (key.ToString().ToLower().Contains("oem"))
            {
                var outputBuilder = new StringBuilder(2);

                int result = ToAscii((uint)key, 0, new byte[0], outputBuilder, 0);
                if (result == 1)
                    return (outputBuilder[0].ToString()).ToUpper();
            }
            return key.ToString();
        }

        [DllImport("user32.dll")]
        private static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState, [Out] StringBuilder lpChar, uint uFlags);

    }
}

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;

namespace Autofire.Support.Utils.Form.InputTransformation
{
    public class KeyTransformer
    {
        private const string Pattern = "[\\s*]?\\+[\\s*]?";

        public Keys KeysFromString(string key)
        {
            var keysConverter = new KeysConverter();

            try
            {
                return (Keys)keysConverter.ConvertFromString(key);
            }
            catch (Exception)
            {
                var keys = Split(key);

                var systemKeys = Keys.None;

                for (var i = 0; i < keys.Length - 1; i++)
                {
                    systemKeys |= (Keys)keysConverter.ConvertFromString(keys[i]);
                }

                var virtualKeyboardKey = VkKeyScan(keys[keys.Length - 1][0]);

                var keyboardKeys = (Keys)(virtualKeyboardKey & 0xff);

                return systemKeys | keyboardKeys;
            }
        }

        public string KeyToString(Key key, ModifierKeys modifierKeys)
        {
            var modifierKeysConverter = new ModifierKeysConverter();
            var modifiers = modifierKeysConverter.ConvertToString(modifierKeys);

            return Join(modifiers, ExtractKey(key));
        }

        private string Join(string key1, string key2)
        {
            var separator = " + ";

            string[] arr = Regex.Split(key1, Pattern)
                .Concat(Regex.Split(key2, Pattern))
                .Where(s => !String.IsNullOrEmpty(s))
                .ToArray();

            return String.Join(separator, arr);
        }

        private string[] Split(string keys)
        {
            return Regex.Split(keys, Pattern);
        }

        private string ExtractKey(Key key)
        {
            key = RemoveModifiers(key);

            if (IsOem(key))
            {
                return OemToString(key);
            }
            else
            {
                var keyConverter = new KeyConverter();
                return keyConverter.ConvertToString(key);
            }
        }

        private Key RemoveModifiers(Key key)
        {
            return Constants.KeyModifiers.Contains(key) ? Key.None : key;
        }

        private bool IsOem(Key key)
        {
            Keys systemKeys = KeyToKeys(key);

            return systemKeys.ToString().ToLower().Contains("oem");
        }

        private string OemToString(Key key)
        {
            var systemKeys = KeyToKeys(key);

            var virtualKeyboardKey = (char)MapVirtualKey((int)systemKeys, 2);

            if (systemKeys.Equals(KeysFromString(virtualKeyboardKey.ToString())))
            {
                return virtualKeyboardKey.ToString().ToUpper();
            }
            else
            {
                return systemKeys.ToString();
            }
        }

        private static Keys KeyToKeys(Key key)
        {
            return (Keys)KeyInterop.VirtualKeyFromKey(key);
        }

        [DllImport("user32.dll")]
        static extern int MapVirtualKey(int uCode, int uMapType);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern short VkKeyScan(char ch);
    }
}

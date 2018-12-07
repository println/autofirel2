using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace Autofire.Support.Utils.Form.InputTransformation
{
    public static class Constants
    {
        public static readonly ISet<Key> KeyModifiers = new HashSet<Key>() {
               Key.System,
               Key.LeftAlt,
               Key.RightAlt,
               Key.LeftCtrl,
               Key.RightCtrl,
               Key.LeftShift,
               Key.RightShift,
               Key.LWin,
               Key.RWin
        };

        public static readonly Keys KeysModifiers =
               Keys.RMenu | Keys.LMenu |
               Keys.LControlKey | Keys.RControlKey |
               Keys.LShiftKey | Keys.RShiftKey |
               Keys.LWin | Keys.RWin;

    }
}

using System.Windows.Input;
using Autofire.Support.Utils.Form.InputTransformation;
using Autofire.Support.Utils.ViewModel;
using Caliburn.Micro;

namespace Autofire.Core.UI.ViewModels
{
    public class MacroModalViewModel : AbstractModalViewModel<MacroModalViewModel>
    {
        private string hotkey;
        public string Hotkey
        {
            get => hotkey;
            set
            {
                Set(ref hotkey, value);
            }
        }

        public override MacroModalViewModel GetContent()
        {
            return this;
        }

        public void ExecuteFilterView(ActionExecutionContext context)
        {
            var args = (KeyEventArgs)context.EventArgs;
            args.Handled = true;

            var key = args.SystemKey == Key.F10 ? Key.F10 : args.Key;
            
            var transform = new KeyTransformer();

            Hotkey = transform.KeyToString(key, Keyboard.Modifiers);
        }
    }
}

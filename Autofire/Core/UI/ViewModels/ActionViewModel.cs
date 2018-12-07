using System;
using System.Media;
using System.Windows.Controls;
using System.Windows.Input;
using Autofire.Core.Features.Profile.Model;
using Autofire.Support.Utils.ViewModel;
using Autofire.Support.Utils.ViewModel.Reactive;
using Caliburn.Micro;

namespace Autofire.Core.UI.ViewModels
{
    public class ActionViewModel : AbstractReactiveViewModel, ViewModel.IActionViewModel
    {
        private IAction model;
        
        public IAction Model
        {
            get => model;
            set => Set(ref model, value, null);
        }

        public string Name
        {
            get => Model.Name;
            set
            {
                Save(Model, () => Model.Name, value);
            }
        }

        public decimal Interval
        {
            get => Model.Interval;
            set
            {
                Save(Model, () => Model.Interval, value);
            }
        }

        public string Key
        {
            get => Model.Key;
            set
            {
                Save(Model, () => Model.Key, value);
            }
        }

        public void PreviewTextInput(ActionExecutionContext context)
        {
            var args = (TextCompositionEventArgs)context.EventArgs;
            var ch = args.Text;
            var source = (TextBox)args.Source;

            int x;
            args.Handled = !int.TryParse(ch, out x);

            if (ch.Equals(".") && !source.Text.Contains("."))
            {
                args.Handled = false;
            }

            if (args.Handled)
            {
                SystemSounds.Beep.Play();
            }
        }

        public bool IsRunning { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CanRun { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    }
}

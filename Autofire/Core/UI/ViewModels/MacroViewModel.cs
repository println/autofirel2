using System;
using System.Collections.ObjectModel;
using Autofire.Core.Features.Profile.Model;
using Autofire.Support.Utils.ViewModel;
using Autofire.Support.Utils.ViewModel.Reactive;

namespace Autofire.Core.UI.ViewModels
{
    public class MacroViewModel : AbstractReactiveViewModel, ViewModel.IMacroViewModel<ActionViewModel>
    {

        private IMacro model;
        public IMacro Model
        {
            get
            {
                return model;
            }
            set
            {
                Set(ref model, value, null);
            }
        }

        public string Name
        {
            get
            {
                return Model.Name;
            }
            set
            {
                Save(Model, () => Model.Name, value);
            }
        }

        public string Hotkey
        {
            get
            {
                return Model.Hotkey;
            }
            set
            {
                Save(Model, () => Model.Hotkey, value);
            }
        }

        public bool IsRepeat
        {
            get
            {
                return Convert.ToBoolean(Model.ExecutionMode);
            }
            set
            {
                Save(Model, () => Model.ExecutionMode, (ExecutionMode)Convert.ToInt32(value));
            }
        }

        private bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                Notify(ref isActive, value);
            }
        }

        public ObservableCollection<ActionViewModel> Actions { get; set; }

        public void ShowHotkeyModal()
        {
            var viewModel = new MacroModalViewModel() { Hotkey = Hotkey };

            viewModel.ShowModal(DoHotkey);
        }

        private void DoHotkey(MacroModalViewModel modal)
        {
            Hotkey = modal.Hotkey;
        }

        public bool IsRunning { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CanRun { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autofire.Core.Features.Profile.Model;

namespace Autofire.Core.UI.ViewModels.Helpers
{
    internal class ModelToViewModel
    {
        internal ProfileViewModel Wrap(IProfile profile)
        {
            return new ProfileViewModel()
            {
                Model = profile,
                Macros = ToMacroViewModel(profile.Macros)
            };
        }

        private ObservableCollection<MacroViewModel> ToMacroViewModel(IEnumerable<IMacro> macros)
        {
            return new ObservableCollection<MacroViewModel>(macros.Select(CreateMacroViewModel));
        }

        private MacroViewModel CreateMacroViewModel(IMacro macro)
        {
            return new MacroViewModel()
            {
                Model = macro,
                Actions = ToActionViewModel(macro.Actions)
            };
        }

        private ObservableCollection<ActionViewModel> ToActionViewModel(IEnumerable<IAction> actions)
        {
            return new ObservableCollection<ActionViewModel>(actions.Select(CreateAction));
        }

        private ActionViewModel CreateAction(IAction action)
        {
            return new ActionViewModel()
            {
                Model = action
            };
        }


        internal void Refresh(ProfileViewModel profileVM, IProfile profile)
        {
            profileVM.Model = profile;
            Refresh(profileVM.Macros, profile.Macros);
        }

        private void Refresh(ObservableCollection<MacroViewModel> macrosVM, ICollection<IMacro> macros)
        {
            var macrosVMEnum = macrosVM.GetEnumerator();
            var macrosEnum = macros.GetEnumerator();

            while (macrosVMEnum.MoveNext() & macrosEnum.MoveNext())
            {
                Refresh(macrosVMEnum.Current, macrosEnum.Current);
            }            
        }

        private void Refresh(MacroViewModel macroVM, IMacro macro)
        {
            macroVM.Model = macro;
            Refresh(macroVM.Actions, macro.Actions);
        }

        private void Refresh(ObservableCollection<ActionViewModel> actionsVM, ICollection<IAction> actions)
        {
            var actionsVMEnum = actionsVM.GetEnumerator();
            var actionsEnum = actions.GetEnumerator();

            while (actionsVMEnum.MoveNext() & actionsEnum.MoveNext())
            {
                actionsVMEnum.Current.Model = actionsEnum.Current;
            }
        }
    }
}
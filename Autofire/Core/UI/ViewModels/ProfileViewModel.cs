using System;
using System.Collections.ObjectModel;
using Autofire.Core.Features.Profile.Model;
using Autofire.Support.Utils.ViewModel;
using Autofire.Support.Utils.ViewModel.Reactive;

namespace Autofire.Core.UI.ViewModels
{
    public class ProfileViewModel : AbstractReactiveViewModel, ViewModel.IProfileViewModel<MacroViewModel, ActionViewModel>
    {
        public IProfile Model { get; set; }

        public string Id => Model.Id;

        public string Name
        {
            get => Model.Name;
            set
            {
                Set(Model, () => this.Model.Name, value);
            }
        }

        public string Description
        {
            get => Model.Description;
            set
            {
                Set(Model, () => this.Model.Description, value);
            }
        }

        public bool IsGlobal
        {
            get
            {
                return Convert.ToBoolean(Model.Coverage);
            }
            set
            {
                Save(Model, () => Model.Coverage, (CoverageMode)Convert.ToInt32(value));
            }
        }

        public ObservableCollection<MacroViewModel> Macros { get; set; }

        public IProfile GetModel()
        {
            return Model;
        }
    }
}

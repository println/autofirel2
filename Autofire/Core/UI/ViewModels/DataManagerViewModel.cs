using System.Collections.ObjectModel;
using System.Linq;
using Autofire.Core.Features;
using Autofire.Properties;
using Autofire.Support.Utils.ViewModel;
using Autofire.Support.Utils.ViewModel.Reactive;
using Caliburn.Micro;

namespace Autofire.Core.UI.ViewModels
{
    public class DataManagerViewModel : AbstractViewModel, IHandle<OnSaveMessage>
    {
        private readonly IProfileManager profileManager;
        private IEventAggregator eventAggregator;

        public ProfileViewModel ProfileVM { get; private set; }

        public int SelectedIndex
        {
            get => this.profileManager.SelectedIndex;
            set => this.profileManager.SelectedIndex = value;
        }

        public ObservableCollection<string> Profiles => this.profileManager.ProfileIdList;

        public DataManagerViewModel(IProfileManager profileManager, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            AbstractReactiveViewModel.EventAggregator = this.eventAggregator; 
            eventAggregator.Subscribe(this);
            
            this.profileManager = profileManager;
            this.profileManager.DataChangedEvent += DataChangedEventHandler;
            this.SetProfileVM();
        }


        public void ReloadScreen()
        {
            //this.profileManager.Refresh();
            NotifyOfPropertyChange(() => this.Profiles);
        }

        public void ShowRenameModal()
        {
            var viewModel = new DataManagerModalViewModel()
            {
                Title = Resources.ConfigRenameTitle,
                Name = ProfileVM.Name,
                Description = ProfileVM.Description
            };
            viewModel.ShowModal(DoRename);
        }

        public void ShowCreateModal()
        {
            var viewModel = new DataManagerModalViewModel()
            {
                Title = Resources.ConfigCreateTitle
            };
            viewModel.ShowModal(DoCreate);
        }

        public void ShowDeleteModal()
        {
            var viewModel = new DataManagerDeleteModalViewModel()
            {
                Id = ProfileVM.Id
            };
            viewModel.ShowModal(DoDelete);
        }

        private void DoRename(DataManagerModalViewModel modal)
        {
            this.profileManager.RenameActiveProfile(modal.Name, modal.Description);
        }

        private void DoCreate(DataManagerModalViewModel modal)
        {
            this.profileManager.CreateProfile(modal.Name, modal.Description);
        }

        private void DoDelete(DataManagerDeleteModalViewModel modal)
        {
            this.profileManager.DeleteActiveProfile();
        }

        public void Handle(OnSaveMessage message)
        {
            this.profileManager.SaveActiveProfile();
        }

        private void DataChangedEventHandler(DataChangedType[] types)
        {
            if (types.Contains(DataChangedType.LIST))
            {
                NotifyOfPropertyChange(() => this.Profiles);
                NotifyOfPropertyChange(() => this.SelectedIndex);
            }

            if (types.Contains(DataChangedType.INDEX))
            {
                NotifyOfPropertyChange(() => this.SelectedIndex);
            }

            if (types.Contains(DataChangedType.PROFILE))
            {
                this.SetProfileVM();
            }
        }

        private void SetProfileVM()
        {
            if (ProfileVM == null)
            {
                ProfileVM = new Helpers.ModelToViewModel().Wrap(this.profileManager.ActiveProfile);
            }
            else
            {
                new Helpers.ModelToViewModel().Refresh(ProfileVM, this.profileManager.ActiveProfile);
            }

            NotifyOfPropertyChange(() => this.ProfileVM);
        }
    }
}
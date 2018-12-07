using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Autofire.Support.Utils.ViewModel;
using Autofire.Support.Utils.ViewModel.Reactive;
using Caliburn.Micro;

namespace Autofire.Core.UI.ViewModels
{
    public class ShellViewModel : AbstractViewModel, IHandle<OnNotifyMessage>
    {
        private IEventAggregator eventAggregator;

        public DataManagerViewModel DataManager { get; private set; }

        public GameViewModel GameBar { get; private set; }

        public bool CanRun => TotalActivated > 0;

        public int TotalActivated
        {
            get
            { return DataManager.ProfileVM.Macros.Where(m => m.IsActive).Count(); }
        }

        public ShellViewModel(IEventAggregator eventAggregator, DataManagerViewModel dataManager, GameViewModel gameBar)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            this.GameBar = gameBar;
            this.DataManager = dataManager;
            this.DataManager.PropertyChanged += ProfileVMChanged;
        }

        public void Run()
        {
            Trace.WriteLine($"Running -> ");
        }

        private void ProfileVMChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DataManager.ProfileVM):
                    NotifyOfPropertyChange(null);
                    break;
            }
        }

        public void Handle(OnNotifyMessage message)
        {
            Trace.WriteLine($"Macro is Notifying changes - > {message.PropertyName}");
            NotifyOfPropertyChange(() => this.TotalActivated);
            NotifyOfPropertyChange(() => this.CanRun);
        }
    }
}

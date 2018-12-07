using System.Collections.ObjectModel;
using Autofire.Core.Features;
using Autofire.Support.Utils.ViewModel;

namespace Autofire.Core.UI.ViewModels
{
    public class GameViewModel : AbstractViewModel
    {
        private readonly IGameManager gameManager;

        public ObservableCollection<int> Games => this.gameManager.GameList;

        public int SelectedIndex
        {
            get => this.gameManager.SelectedIndex;
            set
            {
                if (Set(this.gameManager, () => gameManager.SelectedIndex, value ))
                {
                    NotifyOfPropertyChange("");
                }
            }
        }

        public string Name => $"{this.gameManager.GameName} ({this.Games.Count})";

        public bool CanFocus => this.gameManager.CanRun();

        public GameViewModel(IGameManager gameManager)
        {
            this.gameManager = gameManager;
           this.gameManager.GameList.CollectionChanged += (sender, e) =>
           {
               NotifyOfPropertyChange(nameof(CanFocus));
               NotifyOfPropertyChange(nameof(Name));
           };
        }

        public void DoFocus()
        {
            this.gameManager.DoFocus();
        }
    }
}
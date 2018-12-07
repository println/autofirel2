using System.Collections.ObjectModel;

namespace Autofire.Core.Features
{
    public delegate void GameClosedEventHandler(bool isSelected);

    public interface IGameManager
    {
        event GameClosedEventHandler GameClosedEvent;

        ObservableCollection<int> GameList { get; }
        int SelectedIndex { get; set; }
        string GameName { get; }
        
        int GameProcessId { get; }
        void DoFocus();

        bool CanRun();
    }
}
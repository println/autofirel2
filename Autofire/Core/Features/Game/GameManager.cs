using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Autofire.Config;
using Autofire.Support.Daemons.Game;
using Autofire.Support.Utils.Threading.Window;
using Action = System.Action;

namespace Autofire.Core.Features.Game
{
    public class GameManager : IGameManager
    {
        private readonly IGameDaemon gameDaemon;
        private readonly WindowHandler windowHandler;
        private readonly IList<int> gameProcessIds;

        public event GameClosedEventHandler GameClosedEvent = delegate { };
        public string GameName => AppConfig.GameDisplayName;

        public int GameProcessId
        {
            get
            {
                if (!this.CanRun())
                {
                    throw new IOException("Game is not Running!");
                }

                return this.gameProcessIds[this.SelectedIndex - 1];
            }
        }

        public ObservableCollection<int> GameList { private set; get; }

        public int SelectedIndex { get; set; }


        public GameManager()
        {
            this.windowHandler = new WindowHandler();
            this.gameDaemon = AppConfig.CreateGameDaemon();
            this.gameDaemon.GameStartedEvent += GameOpened;
            this.gameDaemon.GameClosedEvent += GameClosed;
            this.gameProcessIds = new List<int>(this.gameDaemon.Scan().Keys);
            this.GameList = new ObservableCollection<int>();
            this.UpdateGameList();
        }

        public void DoFocus()
        {
            if (this.CanRun())
            {
                var processId = this.gameProcessIds[this.SelectedIndex];
                this.windowHandler.Focus(processId);
            }
        }

        public bool CanRun()
        {
            if (!this.GameList.Any())
            {
                return false;
            }

            if (this.SelectedIndex < 0) 
            {
                return false;
            }
            
            if(this.SelectedIndex >= this.GameList.Count)
            {
                return false;
            }

            return true;
        }

        private void GameClosed(string name, int processId, GameStatus status)
        {
            if (!this.gameProcessIds.Contains(processId))
            {
                return;
            }

            var selectedValue = this.SelectedIndex;
            var isSelected = false;

            if (this.gameProcessIds.IndexOf(processId) == selectedValue)
            {
                selectedValue = -1;
                isSelected = true;
            }
            else if (this.gameProcessIds.Count > 1 && this.gameProcessIds.IndexOf(processId) < selectedValue)
            {
                selectedValue--;
            }

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.SelectedIndex = selectedValue;
                this.gameProcessIds.Remove(processId);
                this.GameList.Remove(this.GameList.Count());
                this.GameClosedEvent(isSelected);
                Trace.WriteLine($"Game Closed -> {processId}");
            }));
        }

        private void GameOpened(string name, int processId, GameStatus status)
        {
            if (this.gameProcessIds.Contains(processId))
            {
                return;
            }

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.gameProcessIds.Add(processId);
                this.GameList.Add(this.GameList.Count() + 1);
                Trace.WriteLine($"New Game Started -> {processId}");
            }));
        }

        private void UpdateGameList()
        {
            for (var i = 0; i < this.gameProcessIds.Count(); i++)
            {
                this.GameList.Add(i + 1);
            }
        }
    }
}
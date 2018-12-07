using System.Collections.Generic;

namespace Autofire.Support.Daemons.Game
{

    public delegate void GameEventHandler(string name, int processId, GameStatus status);

    public enum GameStatus { Started, Closed }

    public interface IGameDaemon : IDaemon
    {
        event GameEventHandler GameClosedEvent;
        event GameEventHandler GameStartedEvent;
       
        IDictionary<int, string> Scan();
    }
}
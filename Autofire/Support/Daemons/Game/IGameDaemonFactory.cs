using System.Diagnostics;

namespace Autofire.Support.Daemons.Game
{
    public interface IGameDaemonFactory
    {
        IGameDaemon CreateGameDaemonByFilename(string name);
        IGameDaemon CreateGameDaemonByFilename(string name, uint intervalInSeconds);
        IGameDaemon CreateGameDaemonByWindowTitle(string title, uint intervalInSeconds);
       
    }
}
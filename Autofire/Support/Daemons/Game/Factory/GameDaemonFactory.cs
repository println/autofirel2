using System.Diagnostics;

namespace Autofire.Support.Daemons.Game.Factory
{
    public class GameDaemonFactory : IGameDaemonFactory
    {
        public IGameDaemon CreateGameDaemonByFilename(string name)
        {
            return new WmiGameDaemon(name);
        }

        public IGameDaemon CreateGameDaemonByFilename(string name, uint intervalInSeconds)
        {
            return new WmiGameDaemon(name, intervalInSeconds);
        }

        public IGameDaemon CreateGameDaemonByWindowTitle(string title, uint intervalInSeconds)
        {
            return new TaskedGameDaemon(title, intervalInSeconds);
        }
       
    }
}

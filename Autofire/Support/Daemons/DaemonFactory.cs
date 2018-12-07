using System.Diagnostics;
using Autofire.Support.Daemons.Game;
using Autofire.Support.Daemons.Game.Factory;
using Autofire.Support.Daemons.Hotkey;

namespace Autofire.Support.Daemons
{
    public class DaemonDaemonFactory:IGameDaemonFactory
    {
        private readonly IGameDaemonFactory gameDaemonFactory = new GameDaemonFactory();

        public IGameDaemon CreateGameDaemonByFilename(string name)
        {
            return gameDaemonFactory.CreateGameDaemonByFilename(name);
        }

        public IGameDaemon CreateGameDaemonByFilename(string name, uint intervalInSeconds)
        {
            return gameDaemonFactory.CreateGameDaemonByFilename(name, intervalInSeconds);
        }

        public IGameDaemon CreateGameDaemonByWindowTitle(string title, uint intervalInSeconds)
        {
            return gameDaemonFactory.CreateGameDaemonByWindowTitle(title, intervalInSeconds);
        }

        public IHotkeyDaemon CreateHotkeyDaemon()
        {
            return new HotkeyDaemon();
        }
    }
}
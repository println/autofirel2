using Autofire.Core.Features.Profile.Builder;
using Autofire.Properties;
using Autofire.Support.Daemons.Game;
using Autofire.Support.Daemons.Game.Factory;
using Autofire.Support.Daemons.Hotkey;
using Autofire.Support.Repositories.Profile;

namespace Autofire.Config
{
    public static class AppConfig
    {
        public static string GameDisplayName => "Lineage II";

        public static IProfileBuilder ProfileBuilder { get; set; }

        static AppConfig()
        {
            ProfileBuilder = CreateProfileBuilder();
        }

        public static IHotkeyDaemon CreateHotkeyDaemon()
        {
            return new HotkeyDaemon();
        }

        public static IGameDaemon CreateGameDaemon()
        {
            var gameDaemon = new GameDaemonFactory()
                .CreateGameDaemonByFilename("notepad.exe", 1);

            gameDaemon.Start();
            return gameDaemon;
        }

        public static IProfileRepository CreateProfileRepository()
        {
            return new ProfileRepository();
        }

        private static IProfileBuilder CreateProfileBuilder()
        {
            var profileBuilder = new ProfileBuilder();

            string[] keys = {"F1", "F2", "F3", "F4"};

            return profileBuilder
                .SetProfileDetails(Resources.ProfileName, Resources.ProfileDescription)
                .AppendMacro(Resources.ProfileMacroName1, "Ctrl + 1", keys)
                .AppendMacro(Resources.ProfileMacroName2, "Ctrl + 2", keys)
                .AppendMacro(Resources.ProfileMacroName3, "Ctrl + 3", keys);
        }
    }
}
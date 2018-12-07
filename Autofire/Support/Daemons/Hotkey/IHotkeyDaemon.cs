using System.Windows.Forms;

namespace Autofire.Support.Daemons.Hotkey
{
    public interface IHotkeyDaemon: IDaemon
    {
        HotkeyResolver Resolver { get; }

        event KeyEventHandler HotkeyCaptured;

        void Start();
        void Stop();
    }
}
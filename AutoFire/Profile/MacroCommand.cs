using System.Windows.Forms;

namespace AutoFire.Profile
{
    public class MacroCommand
    {
        public uint Id { get; }
        public string Name { get; set; }
        public Keys Key { get; set; }
        public uint IntervalInSeconds { get; set; }

        public MacroCommand(uint id)
        {
            Id = id;
        }
    }
}

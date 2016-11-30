using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoFire.Profile
{
    public class Macro
    {
        public uint Id { get; }
        public string Name { get; set; }
        public bool Repeat { get; set; }
        public Keys ActivationKey { get; set; }
        public IList<MacroCommand> Commands { get; }

        public Macro(uint id)
        {
            Id = id;
            Commands = new List<MacroCommand>();
        }

        public void AddCommand(MacroCommand command)
        {
            Commands.Add(command);
        }
    }
}

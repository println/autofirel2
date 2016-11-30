using System.Collections.Generic;

namespace AutoFire.Profile
{
    public class Profile
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public IList<Macro> Macros { get; }

        public Profile()
        {
            Macros = new List<Macro>();
        }

        public void AddMacro(Macro macro)
        {
            Macros.Add(macro);
        }
    }
}

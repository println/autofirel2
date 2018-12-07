using System.Collections.Generic;

namespace Autofire.Core.Features.Profile.Model
{
    public interface IMacro
    {
        string Name { get; set; }
        string Hotkey { get; set; }
        ExecutionMode ExecutionMode { get; set; }
        IList<IAction> Actions { get; set; }

        
    }
    public enum ExecutionMode { OneShot, Loop }
}
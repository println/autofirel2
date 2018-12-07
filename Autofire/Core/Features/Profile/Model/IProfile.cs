using System.Collections.Generic;

namespace Autofire.Core.Features.Profile.Model
{
    public interface IProfile
    {
        string Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        CoverageMode Coverage { get; set; }
        IList<IMacro> Macros { get; set; }  
    }
    
    public enum CoverageMode { Focus, Any }
}
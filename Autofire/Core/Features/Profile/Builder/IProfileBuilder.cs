using Autofire.Core.Features.Profile.Model;

namespace Autofire.Core.Features.Profile.Builder
{
    public interface IProfileBuilder
    {
        IProfileBuilder SetProfileDetails(string name, string description);
        IProfileBuilder AppendMacro(string name, string hotkey, string[] actionKeys);
        IProfile Build();
        
        IProfile Build(string name, string description);
        IProfileBuilder Clone();
    }
}
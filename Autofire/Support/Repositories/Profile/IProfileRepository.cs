using System.Collections.Generic;
using Autofire.Core.Features.Profile;
using Autofire.Core.Features.Profile.Model;

namespace Autofire.Support.Repositories.Profile
{
    public interface IProfileRepository
    {       
        IEnumerable<string> ListAll();
        IEnumerable<IProfile> GetAll();
        IProfile GetById(string id);
        IProfile Create(IProfile profile);
        IProfile Update(string id, IProfile profile);
        void Remove(string id);
        void LegacyToJson(bool ignoreErrors = true);
        bool IsEmpty();
    }
}
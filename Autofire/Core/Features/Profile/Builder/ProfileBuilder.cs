using System.Collections.Generic;
using Autofire.Core.Features.Profile.Model;

namespace Autofire.Core.Features.Profile.Builder
{
    public class ProfileBuilder : IProfileBuilder
    {
        private string profileName = string.Empty;
        private string profileDescription = string.Empty;
        private IList<(string, string, string[])> macroDetails;

        public ProfileBuilder()
        {            
            macroDetails = new List<(string, string, string[])>();
        }

        public IProfileBuilder SetProfileDetails(string name, string description)
        {
            profileName = name;
            profileDescription = description;

            return this;
        }

        public IProfileBuilder AppendMacro(string name, string hotkey, string[] actionKeys)
        {
            macroDetails.Add((name, hotkey, actionKeys));
            return this;
        }

        public IProfile Build(string name, string description)
        {
            return new ProfileFactory().Create(name, description, macroDetails);
        }
        
        public IProfile Build()
        {
            return new ProfileFactory().Create(profileName, profileDescription, macroDetails);
        }

        public IProfileBuilder Clone() {
            return new ProfileBuilder()
            {
                macroDetails = macroDetails
            };
        }
    }
}

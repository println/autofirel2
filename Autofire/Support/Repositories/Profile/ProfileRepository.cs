using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofire.Core.Features.Profile;
using Autofire.Core.Features.Profile.Model;
using Autofire.Support.Repositories.Profile.TypeSwitch;

namespace Autofire.Support.Repositories.Profile
{
    public class ProfileRepository : IProfileRepository
    {
        private string source = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private const string JSON = ".profile.json";
        private const string XML = ".config";

        public bool IsEmpty()
        {
            return !ListAll().Any();
        }

        public IEnumerable<string> ListAll()
        {
            return ListByFileExtension(JSON);
        }

        public IEnumerable<IProfile> GetAll()
        {
            return ListAll().Select(GetById);
        }

        public IProfile GetById(string id)
        {
            return TypeSwitcher.ParseJson(File.ReadAllText(GetPath(id + JSON)));
        }

        public IProfile Create(IProfile profile)
        {
            var id = GenerateFileName(profile);
           
            var description = profile.Description;
            for (var i = 2; HasFile(id); i++)
            {
                profile.Description = description + i;
                id = GenerateFileName(profile);
            }

            if (id.Equals(profile.Id))
            {
                profile.Id = id;
                WriteFile(profile);
                return profile;
            }
            else
            {
                var clonedProfile = TypeSwitcher.ParseJson(TypeSwitcher.ToJson(profile));
                clonedProfile.Id = id;
                WriteFile(clonedProfile);
                profile.Id = id;
                return clonedProfile;
            }
        }

        private void WriteFile(IProfile profile)
        {
            File.WriteAllText(GetPath(profile.Id + JSON), TypeSwitcher.ToJson(profile));
        }

        private string GenerateFileName(IProfile profile)
        {
            var filename = $"{profile.Name}-{profile.Description}";

            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(filename, invalidRegStr, "_");
        }

        public IProfile Update(string id, IProfile profile)
        {
            if (HasFile(GenerateFileName(profile)))
            {
                var oldProfile = GetById(id);
                profile.Name = oldProfile.Name;
                profile.Description = oldProfile.Description;
            }

            Remove(id);
            return Create(profile);
        }

        public void Remove(string id)
        {
            File.Delete(GetPath(id + JSON));
        }

        private bool HasFile(string id)
        {
            return File.Exists(GetPath(id + JSON));
        }

        public void LegacyToJson(bool ignoreErrors = true)
        {
            try
            {
                ListByFileExtension(XML)
                    .ToList()
                    .ForEach(ConvertLegacy);
            }
            catch
            {
                if (!ignoreErrors)
                {
                    throw;
                }
            }
        }

        private void ConvertLegacy(string filename)
        {
            var xml = File.ReadAllText(GetPath(filename + XML));
            try
            {
                Create(TypeSwitcher.ParseLegacy(xml, filename));
            }
            catch
            {
                Trace.WriteLine($"Cannot parse the Legacy file -> {filename}");
            }
        }

        private IEnumerable<string> ListByFileExtension(string criteria)
        {
            return Directory.GetFiles(source, $"*{criteria}", SearchOption.TopDirectoryOnly)
                .Select(path => GetFileNameWithoutExtension(criteria, path));
        }

        private static string GetFileNameWithoutExtension(string criteria, string path)
        {
            return Path.GetFileName(path).Replace(criteria, "");
        }

        private string GetPath(string filename)
        {
            return Path.Combine(source, filename);
        }
    }
}
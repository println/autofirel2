using Autofire.Core.Features.Profile;
using Autofire.Core.Features.Profile.Model;
using Newtonsoft.Json;

namespace Autofire.Support.Repositories.Profile.TypeSwitch
{
    internal static class TypeSwitcher
    {
        internal static string ToJson(IProfile profile)
        {
            return JsonConvert.SerializeObject(
                profile,
                Newtonsoft.Json.Formatting.Indented,
                new Newtonsoft.Json.Converters.StringEnumConverter());
        }

        internal static IProfile ParseJson(string json)
        {
            return JsonConvert.DeserializeObject<IProfile>(json, new Converter());
        }

        internal static IProfile ParseLegacy(string xml, string id)
        {
            return new LegacyType().ParseLegacy(xml, id);
        }
    }
}
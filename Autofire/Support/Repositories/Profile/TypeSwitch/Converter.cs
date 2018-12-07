using System;
using Autofire.Core.Features.Profile;
using Autofire.Core.Features.Profile.Model;
using Action = Autofire.Core.Features.Profile.Model.Action;

namespace Autofire.Support.Repositories.Profile.TypeSwitch
{
    internal class Converter : Newtonsoft.Json.JsonConverter
    {
        private static readonly string ProfileName = typeof(IProfile).FullName;
        private static readonly string MacroName = typeof(IMacro).FullName;
        private static readonly string ActionName = typeof(IAction).FullName;


        public override bool CanConvert(Type objectType)
        {
            return objectType.FullName == ProfileName
                   || objectType.FullName == MacroName
                   || objectType.FullName == ActionName;
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (objectType.FullName == ProfileName)
            {
                return serializer.Deserialize(reader, typeof(Core.Features.Profile.Model.Profile));
            }
            else if (objectType.FullName == MacroName)
            {
                return serializer.Deserialize(reader, typeof(Core.Features.Profile.Model.Macro));
            }
            else if (objectType.FullName == ActionName)
            {
                return serializer.Deserialize(reader, typeof(Action));
            }

            throw new NotSupportedException($"Type {objectType} unexpected.");
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}

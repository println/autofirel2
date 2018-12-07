using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Autofire.Core.Features.Profile;
using Autofire.Core.Features.Profile.Model;

namespace Autofire.Support.Repositories.Profile.TypeSwitch
{
    internal class LegacyType
    {

        internal IProfile ParseLegacy(string xml, string id)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            LegacyData response = null;
            XmlSerializer serializer = new XmlSerializer(typeof(LegacyData));
            using (XmlReader reader = new XmlNodeReader(doc))
            {
                response = (LegacyData)serializer.Deserialize(reader);
            }

            return LegacyToProfile(response, id);
        }

        internal IProfile LegacyToProfile(LegacyData legacy, string id)
        {
            return new Core.Features.Profile.Model.Profile()
            {
                Name = id,
                Description = "legacy",
                Macros = legacy.Macro.Select(CreateKit).ToList()
            };
        }

        private IMacro CreateKit(Macro m)
        {
            if (m.Key_interval.Count == 3)
            {
                m.Key_interval.Add(new Commands()
                {
                    Interval = 100,
                    Key = "0",
                    Name = ""
                });
            }

            return new Core.Features.Profile.Model.Macro()
            {
                Hotkey = m.Activationkey.Winkeys,
                ExecutionMode = m.Loop ? ExecutionMode.Loop : ExecutionMode.OneShot,
                Actions = m.Key_interval.Select(CreateAction).ToList()
            };
        }

        private IAction CreateAction(Commands c)
        {
            return new Action()
            {
                Name = c.Name,
                Key = IndexToFnKey(c.Key),
                Interval = (decimal)(c.Interval / 1000.0)
            };
        }

        private string IndexToFnKey(string c)
        {
            if (c.Equals("0"))
            {
                return "";
            }
            return "F" + c;
        }
    }

    [XmlRoot(ElementName = "key_interval")]
    public class Commands
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "key")]
        public string Key { get; set; }
        [XmlElement(ElementName = "interval")]
        public uint Interval { get; set; }
    }

    [XmlRoot(ElementName = "activation-key")]
    public class Activation
    {
        [XmlElement(ElementName = "label")]
        public string Label { get; set; }
        [XmlElement(ElementName = "winkeys")]
        public string Winkeys { get; set; }
    }

    [XmlRoot(ElementName = "macro")]
    public class Macro
    {
        [XmlElement(ElementName = "key_interval")]
        public List<Commands> Key_interval { get; set; }
        [XmlElement(ElementName = "activation-key")]
        public Activation Activationkey { get; set; }
        [XmlElement(ElementName = "loop")]
        public string Loops
        {
            set
            {
                Loop = bool.Parse(value.ToLower());
            }
            get
            {
                return Loop.ToString();
            }
        }

        [XmlIgnore]
        public bool Loop { get; set; }
    }

    [XmlRoot(ElementName = "AutoFire")]
    public class LegacyData
    {
        [XmlElement(ElementName = "macro")]
        public List<Macro> Macro { get; set; }
    }

}

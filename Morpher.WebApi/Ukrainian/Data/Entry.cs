namespace Morpher.WebService.V3.Ukrainian.Data
{

    using System.Xml.Serialization;
    using General.Data;
    using Newtonsoft.Json;

    public class Entry
    {
        public Entry(DeclensionForms singular)
        {
            Singular = singular;
        }

        public Entry(CorrectionEntry entry)
        {
            Singular = new DeclensionForms(entry.Singular);
        }

        public Entry()
        {
        }

        [XmlIgnore]
        [JsonIgnore]
        public bool GenderXmlSpecified { get; set; } = false;

        [XmlIgnore]
        [JsonIgnore]
        public Gender? Gender
        {
            get { return GenderXmlSpecified ? GenderXml : (Gender?)null; }
            set
            {
                if (value == null)
                {
                    GenderXmlSpecified = false;
                }
                else
                {
                    GenderXmlSpecified = true;
                    GenderXml = value.Value;
                }
            }
        }

        [XmlAttribute("gender")]
        [JsonProperty(PropertyName = "gender", DefaultValueHandling = DefaultValueHandling.Ignore, Order = 0)]
        public Gender GenderXml { get; set; }

        [JsonProperty(PropertyName = "singular", DefaultValueHandling = DefaultValueHandling.Ignore, Order = 1)]
        [XmlElement("singular")]
        public DeclensionForms Singular { get; set; }
    }
}

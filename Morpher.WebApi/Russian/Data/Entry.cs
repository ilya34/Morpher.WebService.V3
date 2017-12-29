namespace Morpher.WebService.V3.Russian.Data
{

    using System.Xml.Serialization;
    using General.Data;
    using Newtonsoft.Json;

    [XmlRoot]
    public class Entry
    {
        public Entry(DeclensionFormsForCorrection singular, DeclensionFormsForCorrection plural)
        {
            Singular = singular;
            Plural = plural;
        }

        public Entry()
        {
        }

        public Entry(CorrectionEntry entry)
        {
            Singular = new DeclensionFormsForCorrection(entry.Singular);
            Plural = new DeclensionFormsForCorrection(entry.Plural);
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
        [JsonProperty(PropertyName = "gender", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Gender GenderXml { get; set; }

        [JsonProperty(PropertyName = "singular", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("singular")]
        public DeclensionFormsForCorrection Singular { get; set; }

        [JsonProperty(PropertyName = "plural", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("plural")]
        public DeclensionFormsForCorrection Plural { get; set; }
    }
}

namespace Morpher.WebService.V3.Russian.Data
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [DataContract]
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
        [JsonProperty]
        public Gender GenderXml { get; set; }

        [DataMember(Name = "singular", EmitDefaultValue = false)]
        [XmlElement("singular")]
        public DeclensionFormsForCorrection Singular { get; set; }

        [DataMember(Name = "plural", EmitDefaultValue = false)]
        [XmlElement("plural")]
        public DeclensionFormsForCorrection Plural { get; set; }
    }
}

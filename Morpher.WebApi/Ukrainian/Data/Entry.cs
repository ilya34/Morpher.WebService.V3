namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using General.Data;
    using Newtonsoft.Json;

    [DataContract]
    [XmlRoot]
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
        [DataMember(Name = "gender", EmitDefaultValue = false)]
        public Gender GenderXml { get; set; }

        [DataMember(Name = "singular", EmitDefaultValue = false)]
        [XmlElement("singular")]
        public DeclensionForms Singular { get; set; }
    }
}

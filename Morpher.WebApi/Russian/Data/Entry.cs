namespace Morpher.WebService.V3.Russian.Data
{
    using System.Xml.Serialization;

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
        public bool GenderXmlSpecified { get; set; } = false;

        [XmlIgnore]
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
        public Gender GenderXml { get; set; }

        [XmlElement("singular")]
        public DeclensionFormsForCorrection Singular { get; set; }

        [XmlElement("plural")]
        public DeclensionFormsForCorrection Plural { get; set; }
    }
}

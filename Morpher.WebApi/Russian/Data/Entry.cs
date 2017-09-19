namespace Morpher.WebService.V3.Russian.Data
{
    using System.Xml.Serialization;

    [XmlRoot]
    public class Entry
    {
        public Entry(DeclensionFormsForCorrection  singular, DeclensionFormsForCorrection plural)
        {
            Singular = singular;
            Plural = plural;
        }

        public Entry()
        {
        }

        [XmlElement("singular")]
        public DeclensionFormsForCorrection Singular { get; set; }

        [XmlElement("plural")]
        public DeclensionFormsForCorrection Plural { get; set; }
    }
}

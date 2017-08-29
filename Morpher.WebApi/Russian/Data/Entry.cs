namespace Morpher.WebService.V3.Russian.Data
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [XmlType("entry")]
    [DataContract]
    public class Entry
    {
        public Entry(DeclensionForms singular, DeclensionForms plural)
        {
            Singular = singular;
            Plural = plural;
        }

        public Entry()
        {
        }

        [XmlElement("singular")]
        [DataMember(Name = "singular", EmitDefaultValue = false)]
        public DeclensionForms Singular { get; set; }

        [XmlElement("plural")]
        [DataMember(Name = "plural", EmitDefaultValue = false)]
        public DeclensionForms Plural { get; set; }
    }
}

namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [XmlType("entry")]
    [DataContract]
    public class RussianEntry
    {
        public RussianEntry(RussianDeclensionForms singular, RussianDeclensionForms plural)
        {
            Singular = singular;
            Plural = plural;
        }

        public RussianEntry()
        {
        }

        [XmlElement("singular")]
        [DataMember(Name = "singular", EmitDefaultValue = false)]
        public RussianDeclensionForms Singular { get; set; }

        [XmlElement("plural")]
        [DataMember(Name = "plural", EmitDefaultValue = false)]
        public RussianDeclensionForms Plural { get; set; }
    }
}

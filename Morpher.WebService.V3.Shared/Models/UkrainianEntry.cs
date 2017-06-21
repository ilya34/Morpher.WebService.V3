namespace Morpher.WebService.V3.Shared.Models
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [XmlType("entry")]
    [DataContract]
    public class UkrainianEntry
    {
        [XmlElement("singular")]
        [DataMember(Name = "singular", EmitDefaultValue = false)]
        public UkrainianDeclensionForms Singular { get; set; }

        [XmlElement("plural")]
        [DataMember(Name = "plural", EmitDefaultValue = false)]
        public UkrainianDeclensionForms Plural { get; set; }
    }
}

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [XmlType("entry")]
    [DataContract]
    public class RussianEntry
    {
        [XmlElement("singular")]
        [DataMember(Name = "singular", EmitDefaultValue = false)]
        public RussianDeclensionForms Singular { get; set; }

        [XmlElement("plural")]
        [DataMember(Name = "plural", EmitDefaultValue = false)]
        public RussianDeclensionForms Plural { get; set; }
    }
}

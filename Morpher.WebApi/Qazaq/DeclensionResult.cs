using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Morpher.WebService.V3.Qazaq
{
    [DataContract(Name = "declension")]
    [XmlRoot("declension")]
    public class DeclensionResult : DeclensionForms
    {
        [DataMember(Name = "көпше", Order = 7, EmitDefaultValue = false)]
        [XmlElement("көпше", Order = 7)]
        public DeclensionForms Plural { get; set; }
    }
}
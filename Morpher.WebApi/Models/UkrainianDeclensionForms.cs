namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType("single-number-forms")]
    public class UkrainianDeclensionForms
    {
        public UkrainianDeclensionForms()
        {
        }

        public UkrainianDeclensionForms(Ukrainian.DeclensionForms serviceResult)
        {
            Nominative = serviceResult.Nominative;
            Genitive = serviceResult.Genitive;
            Dative = serviceResult.Dative;
            Accusative = serviceResult.Accusative;
            Instrumental = serviceResult.Instrumental;
            Prepositional = serviceResult.Prepositional;
            Vocative = serviceResult.Vocative;
        }

        [DataMember(Order = 0, Name = "Н", EmitDefaultValue = false)]
        [XmlElement("И")]
        public string Nominative { get; set; }

        [DataMember(Order = 1, Name = "Р", EmitDefaultValue = false)]
        [XmlElement("Р")]
        public string Genitive { get; set; }

        [DataMember(Order = 2, Name = "Д", EmitDefaultValue = false)]
        [XmlElement("Д")]
        public string Dative { get; set; }

        [DataMember(Order = 3, Name = "З", EmitDefaultValue = false)]
        [XmlElement("З")]
        public string Accusative { get; set; }

        [DataMember(Order = 4, Name = "О", EmitDefaultValue = false)]
        [XmlElement("О")]
        public string Instrumental { get; set; }

        [DataMember(Order = 5, Name = "М", EmitDefaultValue = false)]
        [XmlElement("М")]
        public string Prepositional { get; set; }

        [DataMember(Order = 6, Name = "К", EmitDefaultValue = false)]
        [XmlElement("К")]
        public string Vocative { get; set; }
    }
}
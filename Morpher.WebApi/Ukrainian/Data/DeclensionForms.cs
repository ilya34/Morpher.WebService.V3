namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot]
    public class DeclensionForms
    {    
        /// <summary>
        /// Используется в LibAnalyzer
        /// </summary>
        public DeclensionForms()
        {
        }

        public DeclensionForms(Ukrainian.DeclensionForms serviceResult)
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
        [XmlElement("Н", Order = 0)]
        public string Nominative { get; set; }

        [DataMember(Order = 1, Name = "Р", EmitDefaultValue = false)]
        [XmlElement("Р", Order = 1)]
        public string Genitive { get; set; }

        [DataMember(Order = 2, Name = "Д", EmitDefaultValue = false)]
        [XmlElement("Д", Order = 2)]
        public string Dative { get; set; }

        [DataMember(Order = 3, Name = "З", EmitDefaultValue = false)]
        [XmlElement("З", Order = 3)]
        public string Accusative { get; set; }

        [DataMember(Order = 4, Name = "О", EmitDefaultValue = false)]
        [XmlElement("О", Order = 4)]
        public string Instrumental { get; set; }

        [DataMember(Order = 5, Name = "М", EmitDefaultValue = false)]
        [XmlElement("М", Order = 5)]
        public string Prepositional { get; set; }

        [DataMember(Order = 6, Name = "К", EmitDefaultValue = false)]
        [XmlElement("К", Order = 6)]
        public string Vocative { get; set; }
    }
}
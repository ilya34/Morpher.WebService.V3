using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace Morpher.WebService.V3.Qazaq
{
    [DataContract(Name = "declension")]
    [XmlRoot("declension")]
    public class DeclensionForms
    {
        [DataMember(Name = "A", Order = 0, EmitDefaultValue = false)]
        [XmlElement("A", Order = 0)]
        public string Nominative { get; set; } // Атау септік, именительный падеж

        [DataMember(Name = "І", Order = 1, EmitDefaultValue = false)]
        [XmlElement("І", Order = 1)]
        public string Genitive { get; set; } // Ілік септік, родительный падеж

        [DataMember(Name = "Б", Order = 2, EmitDefaultValue = false)]
        [XmlElement("Б", Order = 2)]
        public string Dative { get; set; }  // Барыс септік, дательно-направительный падеж.

        [DataMember(Name = "Т", Order = 3, EmitDefaultValue = false)]
        [XmlElement("Т", Order = 3)]
        public string Accusative { get; set; } // Табыс септік, винительный падеж

        [DataMember(Name = "Ш", Order = 4, EmitDefaultValue = false)]
        [XmlElement("Ш", Order = 4)]
        public string Ablative { get; set; } // Шығыс септік, исходный падеж

        [DataMember(Name = "Ж", Order = 5, EmitDefaultValue = false)]
        [XmlElement("Ж", Order = 5)]
        public string Locative { get; set; } // Жатыс септік, местный падеж

        [DataMember(Name = "К", Order = 6, EmitDefaultValue = false)]
        [XmlElement("К", Order = 6)]
        public string Instrumental { get; set; } // Көмектес септік, творительный падеж
    }
}
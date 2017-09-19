namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Xml.Serialization;

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

        [XmlElement("Н", Order = 0)]
        public string Nominative { get; set; }

        [XmlElement("Р", Order = 1)]
        public string Genitive { get; set; }

        [XmlElement("Д", Order = 2)]
        public string Dative { get; set; }

        [XmlElement("З", Order = 3)]
        public string Accusative { get; set; }

        [XmlElement("О", Order = 4)]
        public string Instrumental { get; set; }

        [XmlElement("М", Order = 5)]
        public string Prepositional { get; set; }

        [XmlElement("К", Order = 6)]
        public string Vocative { get; set; }
    }
}
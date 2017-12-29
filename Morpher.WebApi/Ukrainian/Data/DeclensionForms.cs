using Newtonsoft.Json;

namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using System.Xml.Serialization;
    using Models;

    [XmlRoot]
    public class DeclensionForms
    {    
        /// <summary>
        /// Используется в LibAnalyzer
        /// </summary>
        public DeclensionForms()
        {
        }

        public DeclensionForms(CorrectionForms correctionForms)
        {
            Nominative = correctionForms.Nominative;
            Vocative = correctionForms.Vocative;
            Accusative = correctionForms.Accusative;
            Dative = correctionForms.Dative;
            Genitive = correctionForms.Genitive;
            Instrumental = correctionForms.Instrumental;
            Prepositional = correctionForms.Prepositional;
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

        public DeclensionForms(List<NameForm> nameForms)
        {
            Nominative = nameForms.SingleOrDefault(form => form.FormID == 'Н')?.AccentedText;
            Genitive = nameForms.SingleOrDefault(form => form.FormID == 'Р')?.AccentedText;
            Accusative = nameForms.SingleOrDefault(form => form.FormID == 'З')?.AccentedText;
            Dative = nameForms.SingleOrDefault(form => form.FormID == 'Д')?.AccentedText;
            Instrumental = nameForms.SingleOrDefault(form => form.FormID == 'О')?.AccentedText;
            Prepositional = nameForms.SingleOrDefault(form => form.FormID == 'М')?.AccentedText;
            Vocative = nameForms.SingleOrDefault(form => form.FormID == 'К')?.AccentedText;
        }

        [JsonProperty(Order = 0, PropertyName = "Н", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("Н", Order = 0)]
        public string Nominative { get; set; }

        [JsonProperty(Order = 1, PropertyName = "Р", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("Р", Order = 1)]
        public string Genitive { get; set; }

        [JsonProperty(Order = 2, PropertyName = "Д", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("Д", Order = 2)]
        public string Dative { get; set; }

        [JsonProperty(Order = 3, PropertyName = "З", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("З", Order = 3)]
        public string Accusative { get; set; }

        [JsonProperty(Order = 4, PropertyName = "О", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("О", Order = 4)]
        public string Instrumental { get; set; }

        [JsonProperty(Order = 5, PropertyName = "М", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("М", Order = 5)]
        public string Prepositional { get; set; }

        [JsonProperty(Order = 6, PropertyName = "К", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("К", Order = 6)]
        public string Vocative { get; set; }
    }
}
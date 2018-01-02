using Newtonsoft.Json;

namespace Morpher.WebService.V3.Russian.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using System.Xml.Serialization;

    public class DeclensionFormsForCorrection
    {
        public DeclensionFormsForCorrection()
        {
        }

        public DeclensionFormsForCorrection(List<Models.NameForm> nameForms)
        {
            Nominative = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
            Genitive = nameForms.SingleOrDefault(form => form.FormID == 'Р')?.AccentedText;
            Accusative = nameForms.SingleOrDefault(form => form.FormID == 'В')?.AccentedText;
            Dative = nameForms.SingleOrDefault(form => form.FormID == 'Д')?.AccentedText;
            Instrumental = nameForms.SingleOrDefault(form => form.FormID == 'Т')?.AccentedText;
            Prepositional = nameForms.SingleOrDefault(form => form.FormID == 'П')?.AccentedText;
            Locative = nameForms.SingleOrDefault(form => form.FormID == 'М')?.AccentedText;
        }

        public DeclensionFormsForCorrection(CorrectionForms correctionForms)
        {
            Nominative = correctionForms.Nominative;
            Genitive = correctionForms.Genitive;
            Dative = correctionForms.Dative;
            Accusative = correctionForms.Accusative;
            Instrumental = correctionForms.Instrumental;
            Prepositional = correctionForms.Prepositional;
            Locative = correctionForms.Locative;
        }

        [JsonProperty(Order = 0, PropertyName = "И", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("И")]
        public string Nominative { get; set; }

        [JsonProperty(Order = 1, PropertyName = "Р", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("Р")]
        public string Genitive { get; set; }

        [JsonProperty(Order = 2, PropertyName = "Д", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("Д")]
        public string Dative { get; set; }

        [JsonProperty(Order = 3, PropertyName = "В", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("В")]
        public string Accusative { get; set; }

        [JsonProperty(Order = 4, PropertyName = "Т", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("Т")]
        public string Instrumental { get; set; }

        [JsonProperty(Order = 5, PropertyName = "П", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("П")]
        public string Prepositional { get; set; }

        [JsonProperty(Order = 6, PropertyName = "М", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("М")]
        public string Locative { get; set; }
    }
}
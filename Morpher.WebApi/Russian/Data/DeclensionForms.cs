using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Morpher.WebService.V3.Russian.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using General.Data;

    public class DeclensionForms
    {
        /// <summary>
        /// Используется в LibAnalyzer
        /// </summary>
        public DeclensionForms()
        {
        }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public DeclensionForms(List<Models.NameForm> nameForms)
        {
            Nominative = nameForms.SingleOrDefault(form => form.FormID == 'И')?.AccentedText;
            Genitive = nameForms.SingleOrDefault(form => form.FormID == 'Р')?.AccentedText;
            Accusative = nameForms.SingleOrDefault(form => form.FormID == 'В')?.AccentedText;
            Dative = nameForms.SingleOrDefault(form => form.FormID == 'Д')?.AccentedText;
            Instrumental = nameForms.SingleOrDefault(form => form.FormID == 'Т')?.AccentedText;
            Prepositional = nameForms.SingleOrDefault(form => form.FormID == 'П')?.AccentedText;
            PrepositionalWithPre = nameForms.SingleOrDefault(form => form.FormID == 'М')?.AccentedText;
        }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public DeclensionForms(Russian.DeclensionForms serviceResult)
        {
            Nominative = serviceResult.Nominative;
            Genitive = serviceResult.Genitive;
            Dative = serviceResult.Dative;
            Accusative = serviceResult.Accusative;
            Instrumental = serviceResult.Instrumental;
            Prepositional = serviceResult.Prepositional;
            PrepositionalWithPre = serviceResult.PrepositionalWithO;
        }

        [JsonProperty(Order = 0, PropertyName = "И", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("И", Order = 0)]
        public string Nominative { get; set; }

        [JsonProperty(Order = 1, PropertyName = "Р", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("Р", Order = 1)]
        public string Genitive { get; set; }

        [JsonProperty(Order = 2, PropertyName = "Д", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("Д", Order = 2)]
        public string Dative { get; set; }

        [JsonProperty(Order = 3, PropertyName = "В", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("В", Order = 3)]
        public string Accusative { get; set; }

        [JsonProperty(Order = 4, PropertyName = "Т", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("Т", Order = 4)]
        public string Instrumental { get; set; }

        [JsonProperty(Order = 5, PropertyName = "П", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("П", Order = 5)]
        public string Prepositional { get; set; }

        [JsonProperty(Order = 6, PropertyName = "П_о", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement("П_о", Order = 6)]
        [OnlyForPaid]
        public string PrepositionalWithPre { get; set; }
    }
}
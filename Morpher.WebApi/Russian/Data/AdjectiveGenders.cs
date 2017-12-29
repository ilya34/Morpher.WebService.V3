using Newtonsoft.Json;

namespace Morpher.WebService.V3.Russian.Data
{

    using System.Xml.Serialization;

    [XmlRoot]
    public class AdjectiveGenders
    {
        public AdjectiveGenders()
        {
        }

        public AdjectiveGenders(Russian.AdjectiveGenders adjectiveGenders)
        {
            Feminie = adjectiveGenders.Feminie;
            Neuter = adjectiveGenders.Neuter;
            Plural = adjectiveGenders.Plural;
        }

        [JsonProperty(PropertyName = "feminine", Order = 0)]
        [XmlElement("feminine", Order = 0)]
        public string Feminie { get; set; }

        [JsonProperty(PropertyName = "neuter", Order = 1)]
        [XmlElement("neuter", Order = 1)]
        public string Neuter { get; set; }

        [JsonProperty(PropertyName = "plural", Order = 2)]
        [XmlElement("plural", Order = 2)]
        public string Plural { get; set; }
    }
}
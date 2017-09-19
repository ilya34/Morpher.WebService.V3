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

        [XmlElement("feminine", Order = 0)]
        public string Feminie { get; set; }

        [XmlElement("neuter", Order = 1)]
        public string Neuter { get; set; }

        [XmlElement("plural", Order = 2)]
        public string Plural { get; set; }
    }
}
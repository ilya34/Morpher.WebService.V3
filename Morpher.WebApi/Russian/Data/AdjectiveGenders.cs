namespace Morpher.WebService.V3.Russian.Data
{
    using System.Runtime.Serialization;

    [DataContract]
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

        [DataMember(Name = "feminine", Order = 0)]
        public string Feminie { get; set; }

        [DataMember(Name = "neuter", Order = 1)]
        public string Neuter { get; set; }

        [DataMember(Name = "plural", Order = 2)]
        public string Plural { get; set; }
    }
}
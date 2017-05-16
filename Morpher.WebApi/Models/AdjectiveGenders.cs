namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class AdjectiveGenders
    {
        [DataMember(Name = "feminine", Order = 0)]
        public string Feminie { get; set; }

        [DataMember(Name = "neuter", Order = 1)]
        public string Neuter { get; set; }

        [DataMember(Name = "plural", Order = 2)]
        public string Plural { get; set; }
    }
}
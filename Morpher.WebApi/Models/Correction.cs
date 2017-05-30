namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "Исправление", Namespace = "")]
    public class Correction
    {
        [DataMember(Name = "Лемма")]
        public string Lemma { get; set; }

        [DataMember(Name = "Падеж")]
        public string Form { get; set; }

        [DataMember(Name = "Множественное")]
        public bool Plural { get; set; }
    }
}
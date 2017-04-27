namespace Morpher.WebApi.Models
{
    using System.Runtime.Serialization;

    using Morpher.WebApi.Models.Interfaces;

    [DataContract]
    public class RussianDeclensionForms : IRussianParadigm
    {
        [DataMember(Order = 0, Name = "И", EmitDefaultValue = false)]
        public string Nominative { get; set; }

        [DataMember(Order = 1, Name = "Р")]
        public string Genitive { get; set; }

        [DataMember(Order = 2, Name = "Д")]
        public string Dative { get; set; }

        [DataMember(Order = 3, Name = "В")]
        public string Accusative { get; set; }

        [DataMember(Order = 4, Name = "Т")]
        public string Instrumental { get; set; }

        [DataMember(Order = 5, Name = "П")]
        public string Prepositional { get; set; }

        [DataMember(Order = 6, Name = "П-о", EmitDefaultValue = false)]
        public string Locative { get; set; }
    }
}
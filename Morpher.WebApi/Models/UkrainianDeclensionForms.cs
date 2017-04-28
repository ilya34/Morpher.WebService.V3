namespace Morpher.WebApi.Models
{
    using System.Runtime.Serialization;

    using Morpher.WebApi.Models.Interfaces;

    [DataContract]
    public class UkrainianDeclensionForms : IUkrainianParadigm
    {
        [DataMember(Order = 0, Name = "Н", EmitDefaultValue = false)]
        public string Nominative { get; set; }

        [DataMember(Order = 1, Name = "Р", EmitDefaultValue = false)]
        public string Genitive { get; set; }

        [DataMember(Order = 2, Name = "Д", EmitDefaultValue = false)]
        public string Dative { get; set; }

        [DataMember(Order = 3, Name = "З", EmitDefaultValue = false)]
        public string Accusative { get; set; }

        [DataMember(Order = 4, Name = "О", EmitDefaultValue = false)]
        public string Instrumental { get; set; }

        [DataMember(Order = 5, Name = "М", EmitDefaultValue = false)]
        public string Prepositional { get; set; }

        [DataMember(Order = 6, Name = "К", EmitDefaultValue = false)]
        public string Vocative { get; set; }
    }
}
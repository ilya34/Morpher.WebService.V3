namespace Morpher.WebApi.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "PropisUkrResult")]
    public class UkrainianNumberSpelling
    {
        [DataMember(Name = "n", Order = 0)]
        public UkrainianDeclensionForms NumberDeclension { get; set; }

        [DataMember(Name = "unit", Order = 1)]
        public UkrainianDeclensionForms UnitDeclension { get; set; }
    }
}
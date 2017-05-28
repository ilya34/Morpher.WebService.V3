namespace Morpher.WebSerivce.V3.Shared.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "PropisResult")]
    public class RussianNumberSpelling
    {
        [DataMember(Name = "n", Order = 0)]
        public RussianDeclensionForms NumberDeclension { get; set; }

        [DataMember(Name = "unit", Order = 1)]
        public RussianDeclensionForms UnitDeclension { get; set; }
    }
}
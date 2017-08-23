namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "PropisResult")]
    public class RussianNumberSpelling
    {
        public RussianNumberSpelling(RussianDeclensionForms number, RussianDeclensionForms unit)
        {
            NumberDeclension = number;
            UnitDeclension = unit;
        }

        [DataMember(Name = "n", Order = 0)]
        public RussianDeclensionForms NumberDeclension { get; set; }

        [DataMember(Name = "unit", Order = 1)]
        public RussianDeclensionForms UnitDeclension { get; set; }
    }
}
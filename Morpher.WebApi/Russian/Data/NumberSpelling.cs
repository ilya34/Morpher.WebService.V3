namespace Morpher.WebService.V3.Russian.Data
{
    using System.Runtime.Serialization;

    [DataContract(Name = "PropisResult")]
    public class NumberSpelling
    {
        public NumberSpelling()
        {
        }

        public NumberSpelling(DeclensionForms number, DeclensionForms unit)
        {
            NumberDeclension = number;
            UnitDeclension = unit;
        }

        [DataMember(Name = "n", Order = 0)]
        public DeclensionForms NumberDeclension { get; set; }

        [DataMember(Name = "unit", Order = 1)]
        public DeclensionForms UnitDeclension { get; set; }
    }
}
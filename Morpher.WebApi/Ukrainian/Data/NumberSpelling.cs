namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Xml.Serialization;

    [DataContract(Name = "PropisUkrResult")]
    [XmlRoot("PropisUkrResult")]
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
        [XmlElement("n")]
        public DeclensionForms NumberDeclension { get; set; }

        [DataMember(Name = "unit", Order = 1)]
        [XmlElement("unit")]
        public DeclensionForms UnitDeclension { get; set; }
    }
}
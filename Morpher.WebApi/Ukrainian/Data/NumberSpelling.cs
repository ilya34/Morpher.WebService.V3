namespace Morpher.WebService.V3.Ukrainian.Data
{
    using System.Xml.Serialization;

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

        [XmlElement("n")]
        public DeclensionForms NumberDeclension { get; set; }

        [XmlElement("unit")]
        public DeclensionForms UnitDeclension { get; set; }
    }
}
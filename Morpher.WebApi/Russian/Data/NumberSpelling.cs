namespace Morpher.WebService.V3.Russian.Data
{
    using System.Xml.Serialization;
    using General.Data;

    [XmlRoot("PropisResult")]
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
        [CheckForPayed]
        public DeclensionForms NumberDeclension { get; set; }

        [XmlElement("unit")]
        [CheckForPayed]
        public DeclensionForms UnitDeclension { get; set; }
    }
}
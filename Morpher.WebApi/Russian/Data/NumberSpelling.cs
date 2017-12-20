namespace Morpher.WebService.V3.Russian.Data
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using General.Data;

    [DataContract(Name = "PropisResult")]
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

        [DataMember(Name = "n", Order = 0)]
        [XmlElement("n")]
        [CheckForPaid]
        public DeclensionForms NumberDeclension { get; set; }

        [DataMember(Name = "unit", Order = 1)]
        [XmlElement("unit")]
        [CheckForPaid]
        public DeclensionForms UnitDeclension { get; set; }
    }
}
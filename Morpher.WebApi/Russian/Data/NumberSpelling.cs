using Newtonsoft.Json;

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

        [JsonProperty(PropertyName = "n", Order = 0)]
        [XmlElement("n")]
        [CheckForPaid]
        public DeclensionForms NumberDeclension { get; set; }

        [JsonProperty(PropertyName = "unit", Order = 1)]
        [XmlElement("unit")]
        [CheckForPaid]
        public DeclensionForms UnitDeclension { get; set; }
    }
}
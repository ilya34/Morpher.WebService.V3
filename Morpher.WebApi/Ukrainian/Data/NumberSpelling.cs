using Newtonsoft.Json;

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

        [JsonProperty(PropertyName = "n", Order = 0)]
        [XmlElement("n")]
        public DeclensionForms NumberDeclension { get; set; }

        [JsonProperty(PropertyName = "unit", Order = 1)]
        [XmlElement("unit")]
        public DeclensionForms UnitDeclension { get; set; }
    }
}
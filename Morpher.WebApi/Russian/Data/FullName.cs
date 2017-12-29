using Newtonsoft.Json;

namespace Morpher.WebService.V3.Russian.Data
{

    using System.Xml.Serialization;

    [XmlRoot]
    public class FullName
    {
        public FullName()
        {
        }

        public FullName(Russian.FullName fullName)
        {
            Surname = fullName.Surname;
            PropertyName = fullName.Name;
            SecondName = fullName.Pantronymic;
        }

        [JsonProperty(PropertyName = "Ф", Order = 0)]
        [XmlElement("Ф", Order = 14)]
        public string Surname { get; set; }

        [JsonProperty(PropertyName = "И", Order = 1)]
        [XmlElement("И", Order = 15)]
        public string PropertyName { get; set; }

        [JsonProperty(PropertyName = "О", Order = 2)]
        [XmlElement("О", Order = 16)]
        public string SecondName { get; set; }
    }
}
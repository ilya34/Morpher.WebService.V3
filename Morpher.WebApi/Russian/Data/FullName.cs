using Newtonsoft.Json;

namespace Morpher.WebService.V3.Russian.Data
{

    using System.Xml.Serialization;

    public class FullName
    {
        public FullName()
        {
        }

        public FullName(Russian.FullName fullName)
        {
            Surname = fullName.Surname;
            Name = fullName.Name;
            SecondName = fullName.Pantronymic;
        }

        [JsonProperty(PropertyName = "Ф", Order = 0)]
        [XmlElement("Ф", Order = 0)]
        public string Surname { get; set; }

        [JsonProperty(PropertyName = "И", Order = 1)]
        [XmlElement("И", Order = 1)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "О", Order = 2)]
        [XmlElement("О", Order = 2)]
        public string SecondName { get; set; }
    }
}
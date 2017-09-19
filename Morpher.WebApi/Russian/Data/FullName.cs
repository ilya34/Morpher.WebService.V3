namespace Morpher.WebService.V3.Russian.Data
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot]
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

        [DataMember(Name = "Ф", Order = 0)]
        [XmlElement("Ф", Order = 14)]
        public string Surname { get; set; }

        [DataMember(Name = "И", Order = 1)]
        [XmlElement("И", Order = 15)]
        public string Name { get; set; }

        [DataMember(Name = "О", Order = 2)]
        [XmlElement("О", Order = 16)]
        public string SecondName { get; set; }
    }
}
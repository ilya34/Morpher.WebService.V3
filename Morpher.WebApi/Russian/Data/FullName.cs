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
            Name = fullName.Name;
            SecondName = fullName.Pantronymic;
        }

        [XmlElement("Ф", Order = 14)]
        public string Surname { get; set; }

        [XmlElement("И", Order = 15)]
        public string Name { get; set; }

        [XmlElement("О", Order = 16)]
        public string SecondName { get; set; }
    }
}
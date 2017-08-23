namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;

    [DataContract]
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
        public string Surname { get; set; }

        [DataMember(Name = "И", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "О", Order = 2)]
        public string SecondName { get; set; }
    }
}
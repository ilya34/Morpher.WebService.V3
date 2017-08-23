namespace Morpher.WebService.V3.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class FullName
    {
        [DataMember(Name = "Ф", Order = 0)]
        public string Surname { get; set; }

        [DataMember(Name = "И", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "О", Order = 2)]
        public string Pantronymic { get; set; }
    }
}
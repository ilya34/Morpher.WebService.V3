// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models
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
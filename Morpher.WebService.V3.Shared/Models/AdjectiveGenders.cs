// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class AdjectiveGenders
    {
        [DataMember(Name = "feminine", Order = 0)]
        public string Feminie { get; set; }

        [DataMember(Name = "neuter", Order = 1)]
        public string Neuter { get; set; }

        [DataMember(Name = "plural", Order = 2)]
        public string Plural { get; set; }
    }
}
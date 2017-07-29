// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "PropisResult")]
    public class RussianNumberSpelling
    {
        [DataMember(Name = "n", Order = 0)]
        public RussianDeclensionForms NumberDeclension { get; set; }

        [DataMember(Name = "unit", Order = 1)]
        public RussianDeclensionForms UnitDeclension { get; set; }
    }
}
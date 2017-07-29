// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Shared.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = "xml")]
    public class RussianDeclensionResult : RussianDeclensionForms
    {
        [DataMember(Name = "род", EmitDefaultValue = false, Order = 7)]
        public string Gender { get; set; }

        [DataMember(Name = "множественное", EmitDefaultValue = false, Order = 8)]
        public RussianDeclensionForms Plural { get; set; }

        [DataMember(Name = "ФИО", EmitDefaultValue = false, Order = 13)]
        public FullName FullName { get; set; }

        [DataMember(Order = 10, Name = "где", EmitDefaultValue = false)]
        public string Where { get; set; }

        [DataMember(Order = 11, Name = "куда", EmitDefaultValue = false)]
        public string To { get; set; }

        [DataMember(Order = 12, Name = "откуда", EmitDefaultValue = false)]
        public string From { get; set; }
    }
}